using UnityEngine;

namespace RPGToolkit
{
    public class CollectQuestStep : QuestStep
    {
        public ItemInfoSO itemToCollect;
        public int amountToComplete = 0;
        public int amountCollected = 0;

        private void OnEnable()
        {
            EventsManager.Instance.collectEvents.onItemCollected += ItemCollected;
        }

        private void OnDisable()
        {
            EventsManager.Instance.collectEvents.onItemCollected -= ItemCollected;
        }

        private void ItemCollected(ItemInfoSO itemCollected)
        {
            Debug.Log("Item Collected : " + itemToCollect.itemName);

            if (itemCollected.itemName == itemToCollect.itemName)
            {
                if (amountCollected < amountToComplete)
                {
                    amountCollected++;
                    UpdateState();
                }

                if (amountCollected >= amountToComplete)
                {
                    FinishQuestStep();
                    return;
                }
            }
        }

        private void UpdateState()
        {
            string state = amountCollected.ToString();
            string status = "Collected " + amountCollected + " / " + amountCollected + " : " + itemToCollect.itemName + ".";
            ChangeState(state, status);
        }

        protected override void SetQuestStepState(string state)
        {
            this.amountCollected = System.Int32.Parse(state);
            UpdateState();
        }
    }
}
