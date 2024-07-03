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

            questManager.saveQuestState = RPGToolkitAsset.saveQuest;
            questManager.loadQuestState = RPGToolkitAsset.loadQuest;
        }
    }
}
