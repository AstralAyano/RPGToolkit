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
                EventsManager.instance.questEvents.AdvanceQuest(questID);
                Destroy(this.gameObject);
            }
        }

        protected void ChangeState(string newState, string newStatus)
        {
            EventsManager.instance.questEvents.QuestStepStateChange(
                questID, 
                stepIndex, 
                new QuestStepState(newState, newStatus)
            );
        }

        protected abstract void SetQuestStepState(string state);
    }
}
