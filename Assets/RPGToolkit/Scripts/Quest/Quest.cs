using UnityEngine;

namespace RPGToolkit
{
    public class Quest : MonoBehaviour
    {
        public QuestInfoSO info;
        public QuestState state;
        public int currentQuestStepIndex;

        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestState.REQUIREMENTS_NOT_MET;
            this.currentQuestStepIndex = 0;
        }

        public void MoveToNextStep()
        {
            currentQuestStepIndex++;
        }

        public bool CurrenStepExists()
        {
            return (currentQuestStepIndex < info.questSteps.Length);
        }

        public void InstantiateCurrentQuestStep(Transform parent)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();

            if (questStepPrefab != null)
            {
                Object.Instantiate<GameObject>(questStepPrefab, parent);
            }
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;

            if (CurrenStepExists())
            {
                questStepPrefab = info.questSteps[currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("StepIndex out of range.");
            }

            return questStepPrefab;
        }
    }
}
