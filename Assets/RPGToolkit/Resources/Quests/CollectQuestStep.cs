using UnityEngine;

namespace RPGToolkit
{
    public class CollectQuestStep : QuestStep
    {
        public ItemInfoSO itemToCollect;
        [SerializeField] private int amountToComplete = 0;
        [SerializeField] private int amountCollected = 0;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {

        }

        private void ItemCollected()
        {
            if (amountCollected < amountToComplete)
            {
                amountCollected++;
            }

            if (amountCollected >= amountToComplete)
            {
                FinishQuestStep();
            }
        }
    }
}
