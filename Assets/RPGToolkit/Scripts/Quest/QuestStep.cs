using UnityEngine;

namespace RPGToolkit
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;
        private string questID;
        private int stepIndex;

        public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
        {
            this.questID = questId;
            this.stepIndex = stepIndex;
            
            if (questStepState != null && questStepState != "")
            {
                SetQuestStepState(questStepState);
            }
        }

        protected void FinishQuestStep()
        {
            if (!isFinished)
            {
                isFinished = true;
                EventsManager.Instance.questEvents.AdvanceQuest(questID);
                Destroy(this.gameObject);
            }
        }

        protected void ChangeState(string newState, string newStatus)
        {
            EventsManager.Instance.questEvents.QuestStepStateChange(
                questID, 
                stepIndex, 
                new QuestStepState(newState, newStatus)
            );
        }

        protected abstract void SetQuestStepState(string state);
    }
}
