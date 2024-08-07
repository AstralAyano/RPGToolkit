using UnityEngine;

namespace RPGToolkit
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "RPGToolkit/New Quest Info", order = 13)]
    public class QuestInfoSO : ScriptableObject
    {
        public enum QuestType
        {
            COLLECTING,
            KILLING,
            TRACKING
        }

        [field: SerializeField] public string questID { get; set; }

        [Header("General")]
        public string questName;
        public QuestType questType;

        [Header("Rewards")]
        [Tooltip("Input '0' or leave it blank if not applicable.")]
        public float currencyReward;

        [Tooltip("Input '0' or leave it blank if not applicable.")]
        public float experienceReward;

        [Header("Requirements")]
        [Tooltip("Input '0' or leave it blank if not applicable.")]
        public int playerLevelRequirement;
        public QuestInfoSO[] questPrerequisites;

        [Header("Steps")]
        public GameObject[] questSteps = new GameObject[0];

        private void OnValidate()
        {
            #if UNITY_EDITOR
            questID = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
