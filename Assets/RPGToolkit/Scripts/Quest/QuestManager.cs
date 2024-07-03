using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        public bool saveQuestState;
        public bool loadQuestState;
        public Dictionary<string, Quest> questMap;

        private int currentPlayerLevel;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            questMap = CreateQuestMap();
        }

        private void Start()
        {
            foreach (Quest quest in questMap.Values)
            {
                // Initialize any loaded quest steps
                if (quest.state == QuestState.IN_PROGRESS)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
                
                // Broadcast the initial state of all quests on startup
                EventsManager.instance.questEvents.QuestStateChange(quest);
            }
        }

        private void OnEnable()
        {
            EventsManager.instance.questEvents.onStartQuest += StartQuest;
            EventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
            EventsManager.instance.questEvents.onFinishQuest += FinishQuest;

            EventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

            EventsManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;
        }

        private void OnDisable()
        {
            EventsManager.instance.questEvents.onStartQuest -= StartQuest;
            EventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            EventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

            EventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

            EventsManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
        }

        private void Update()
        {
            // Loop through ALL quests
            foreach (Quest quest in questMap.Values)
            {
                // If we're now meeting the requirements, switch over to the CAN_START state
                if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.info.questID, QuestState.CAN_START);
                }
            }
        }

        private void PlayerLevelChange(int level)
        {
            currentPlayerLevel = level;
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            // Start true and prove to be false
            bool meetsRequirements = true;

            // Check player level requirements
            if (PlayerController.instance != null && PlayerController.instance.hasLevel)
            {
                if (currentPlayerLevel < quest.info.playerLevelRequirement)
                {
                    meetsRequirements = false;
                }
            }

            // Check quest prerequisites for completion
            foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
            {
                if (GetQuestByID(prerequisiteQuestInfo.questID).state != QuestState.FINISHED)
                {
                    meetsRequirements = false;
                    // Add this break statement here so that we don't continue on to the next quest, since we've proven meetsRequirements to be false at this point.
                    break;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            Quest quest = GetQuestByID(id);
            quest.state = state;
            EventsManager.instance.questEvents.QuestStateChange(quest);
        }

        private void StartQuest(string id) 
        {
            Quest quest = GetQuestByID(id);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.questID, QuestState.IN_PROGRESS);

            Debug.Log("Start Quest : " + quest.info.questID);
        }

        private void AdvanceQuest(string id)
        {
            Quest quest = GetQuestByID(id);

            // Move on to the next step
            quest.MoveToNextStep();

            // If there are more steps, instantiate the next one
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // If there are no more steps, then we've finished all of them for this quest
            else
            {
                ChangeQuestState(quest.info.questID, QuestState.CAN_FINISH);
            }
        }

        private void FinishQuest(string id)
        {
            Quest quest = GetQuestByID(id);
            // Distribute rewards
            ChangeQuestState(quest.info.questID, QuestState.FINISHED);
        }

        private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
        {
            Quest quest = GetQuestByID(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.state);
        }

        private Dictionary<string, Quest> CreateQuestMap()
        {
            // Load all QuestInfoSO ScriptableObjects from Resources folder
            QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

            foreach (QuestInfoSO questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.questID))
                {
                    Debug.LogWarning("Quests : Duplicate ID found when creating Quest Map : " + questInfo.questID);
                }

                idToQuestMap.Add(questInfo.questID, LoadQuest(questInfo));
            }

            return idToQuestMap;
        }

        public Quest GetQuestByID(string id)
        {
            Quest quest = questMap[id];

            if (quest == null)
            {
                Debug.LogError("ID not found in Quest Map : " + id);
            }

            return quest;
        }

        private void OnApplicationQuit()
        {
            foreach (Quest quest in questMap.Values)
            {
                SaveQuest(quest);
                Debug.Log("Saving Quest : " + quest.info.questID);
            }
        }

        private void SaveQuest(Quest quest)
        {
            try 
            {
                if (saveQuestState)
                {
                    QuestData questData = quest.GetQuestData();
                    // serialize using JsonUtility, but use whatever you want here (like JSON.NET)
                    string serializedData = JsonUtility.ToJson(questData);
                    // saving to PlayerPrefs is just a quick example for this tutorial video,
                    // you probably don't want to save this info there long-term.
                    // instead, use an actual Save & Load system and write to a file, the cloud, etc..
                    PlayerPrefs.SetString(quest.info.questID, serializedData);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with ID " + quest.info.questID + " : " + e);
            }
        }

        private Quest LoadQuest(QuestInfoSO questInfo)
        {
            Quest quest = null;

            try 
            {
                // Load quest from saved data
                if (PlayerPrefs.HasKey(questInfo.questID) && loadQuestState)
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.questID);
                    QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                    quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
                }
                // Otherwise, initialize a new quest
                else
                {
                    quest = new Quest(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load quest with ID " + quest.info.questID + " : " + e);
            }

            return quest;
        }
    }
}
