using UnityEngine;

namespace RPGToolkit
{
    public class Quest
    {
        public QuestInfoSO info;
        public QuestState state;
        public int currentQuestStepIndex;
        private QuestStepState[] questStepStates;

        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestState.REQUIREMENTS_NOT_MET;
            this.currentQuestStepIndex = 0;
            this.questStepStates = new QuestStepState[info.questSteps.Length];

            for (int i = 0; i < questStepStates.Length; i++)
            {
                questStepStates[i] = new QuestStepState();
            }
        }

        public Quest(QuestInfoSO questInfo, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
        {
            this.info = questInfo;
            this.state = questState;
            this.currentQuestStepIndex = currentQuestStepIndex;
            this.questStepStates = questStepStates;

            // If the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this.questStepStates.Length != this.info.questSteps.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                    + "of different lengths. This indicates something changed "
                    + "with the QuestInfo and the saved data is now out of sync. "
                    + "Reset your data - as this might cause issues. QuestID: " + this.info.questID);
            }
        }

        public void MoveToNextStep()
        {
            currentQuestStepIndex++;
        }

        public bool CurrentStepExists()
        {
            return (currentQuestStepIndex < info.questSteps.Length);
        }

        public void InstantiateCurrentQuestStep(Transform parent)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();

            if (questStepPrefab != null)
            {
                QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parent).GetComponent<QuestStep>();
                questStep.InitializeQuestStep(info.questID, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
            }
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;

            if (CurrentStepExists())
            {
                questStepPrefab = info.questSteps[currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("StepIndex out of range.");
            }

            return questStepPrefab;
        }

        public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
        {
            if (stepIndex < questStepStates.Length)
            {
                questStepStates[stepIndex].state = questStepState.state;
                questStepStates[stepIndex].status = questStepState.status;
            }
            else 
            {
                Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range : " + "Quest ID : " + info.questID + ", Step Index : " + stepIndex);
            }
        }

        public QuestData GetQuestData()
        {
            return new QuestData(state, currentQuestStepIndex, questStepStates);
        }
    }
}
