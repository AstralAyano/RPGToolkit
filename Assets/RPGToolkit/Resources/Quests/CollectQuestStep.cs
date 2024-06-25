using UnityEngine;

namespace RPGToolkit
{
    public class CollectQuestStep : QuestStep
    {
        public ItemInfoSO itemToCollect;
        [SerializeField] private int amountToComplete = 0;
        [SerializeField] private int amountCollected = 0;

        private void Start()
        {
            QuestManager.instance.collectEvents.onItemCollected += ItemCollected;
        }

        private void OnDisable()
        {
            QuestManager.instance.collectEvents.onItemCollected -= ItemCollected;
        }

        private void ItemCollected(ItemInfoSO itemCollected)
        {
            Debug.Log("ItemCollected Called");

            if (itemCollected.itemName == itemToCollect.itemName)
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
}
