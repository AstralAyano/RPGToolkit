using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;
        public CollectEvents collectEvents;

        private Dictionary<string, Quest> questMap;

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

            collectEvents = new CollectEvents();

            questMap = CreateQuestMap();

            // To Test
            Quest quest = GetQuestByID("CollectPotionQuest");
            Debug.Log(quest.info.questName);
            Debug.Log(quest.info.playerLevelRequirement);
            Debug.Log(quest.state);
            Debug.Log(quest.CurrentStepExists());
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
