using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class QuestManager : MonoBehaviour
    {
        private Dictionary<string, Quest> questMap;

        private int currentPlayerLevel;

        private void Start()
        {
            EventsManager.instance.questEvents.onStartQuest += StartQuest;
            EventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
            EventsManager.instance.questEvents.onFinishQuest += FinishQuest;

            //EventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

            EventsManager.instance.playerEvents.onPlayerLevelChange += PlayerLevelChange;

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

        private void OnDisable()
        {
            EventsManager.instance.questEvents.onStartQuest -= StartQuest;
            EventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            EventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

            //EventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

            EventsManager.instance.playerEvents.onPlayerLevelChange -= PlayerLevelChange;
        }

        private void Awake()
        {
            questMap = CreateQuestMap();
        }

        // private void Start()
        // {
        //     foreach (Quest quest in questMap.Values)
        //     {
        //         // Initialize any loaded quest steps
        //         if (quest.state == QuestState.IN_PROGRESS)
        //         {
        //             quest.InstantiateCurrentQuestStep(this.transform);
        //         }
                
        //         // Broadcast the initial state of all quests on startup
        //         EventsManager.instance.questEvents.QuestStateChange(quest);
        //     }
        // }

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
            if (currentPlayerLevel < quest.info.playerLevelRequirement)
            {
                meetsRequirements = false;
            }

            // Check quest prerequisites for completion
            foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
            {
                if (GetQuestByID(prerequisiteQuestInfo.questID).state != QuestState.FINISHED)
                {
                    meetsRequirements = false;
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

                idToQuestMap.Add(questInfo.questID, new Quest(questInfo));
            }

            return idToQuestMap;
        }

        private Quest GetQuestByID(string id)
        {
            Quest quest = questMap[id];

            if (quest == null)
            {
                Debug.LogError("ID not found in Quest Map : " + id);
            }

            return quest;
        }
    }
}
