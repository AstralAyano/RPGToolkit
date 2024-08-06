using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace RPGToolkit
{
    public class QuestUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject contentParent;
        [SerializeField] private GameObject questItemPrefab;
        [SerializeField] private List<GameObject> questItem;

        private void Start()
        {
            if (!QuestManager.Instance.hasQuestTrackUI)
            {
                gameObject.SetActive(false);
                return;
            }

            EventsManager.Instance.questEvents.onStartQuest += StartQuest;
            EventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
            EventsManager.Instance.questEvents.onFinishQuest += FinishQuest;

            LoadQuest();
        }

        private void OnDisable()
        {
            EventsManager.Instance.questEvents.onStartQuest -= StartQuest;
            EventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
            EventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;
        }

        private void LoadQuest()
        {
            foreach (Quest quest in QuestManager.Instance.questMap.Values)
            {
                if (quest.state == QuestState.IN_PROGRESS)
                {
                    SpawnQuestTrackUI(quest);
                }
            }
        }

        private void StartQuest(string id)
        {
            Quest quest = QuestManager.Instance.GetQuestByID(id);
            SpawnQuestTrackUI(quest);    
        }

        private void FinishQuest(string id)
        {
            Quest quest = QuestManager.Instance.GetQuestByID(id);
            
            foreach (GameObject obj in questItem)
            {
                string questName = obj.transform.Find("QuestName").GetComponent<TMP_Text>().text;

                if (questName == quest.info.questName)
                {
                    questItem.Remove(obj);
                    Destroy(obj);
                    return;
                }
            }
        }

        private void SpawnQuestTrackUI(Quest quest)
        {
            if (QuestManager.Instance.hasQuestTrackUI && quest != null && quest.state == QuestState.IN_PROGRESS)
            {
                // Instantiate a row in QuestUI
                GameObject item = Instantiate(questItemPrefab, contentParent.transform);
                questItem.Add(item);

                TMP_Text questName = item.transform.Find("QuestName").GetComponent<TMP_Text>();
                questName.text = quest.info.questName;
            }
        }

        private void QuestStateChange(Quest quest)
        {
            // Change Quest Icon
        }

        private void SetQuestLogInfo(Quest quest)
        {
            
        }
    }
}
