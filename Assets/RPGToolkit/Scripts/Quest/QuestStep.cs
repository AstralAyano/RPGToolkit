using UnityEngine;

namespace RPGToolkit
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;

        protected void FinishQuestStep()
        {
            if (!isFinished)
            {
                isFinished = true;

                // Message boardcast for advancing to next quest step.

                Destroy(this.gameObject);
            }
        }
    }
}
