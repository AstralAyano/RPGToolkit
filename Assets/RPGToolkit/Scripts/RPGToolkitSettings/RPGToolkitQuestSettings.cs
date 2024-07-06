using UnityEngine;

namespace RPGToolkit
{
    public class RPGToolkitQuestSettings : MonoBehaviour
    {
        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;
        private QuestManager questManager;

        private void Awake()
        {
            questManager = GetComponent<QuestManager>();

            if (questManager != null)
            {
                questManager.hasQuestTrackUI = RPGToolkitAsset.hasQuestTrackUI;
                questManager.hasQuestBookUI = RPGToolkitAsset.hasQuestBookUI;
                questManager.saveQuestState = RPGToolkitAsset.saveQuest;
                questManager.loadQuestState = RPGToolkitAsset.loadQuest;
            }
        }
    }
}
