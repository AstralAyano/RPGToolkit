using UnityEngine;
using System;

namespace RPGToolkit
{
    public class CollectQuestStep : QuestStep
    {
        public ItemInfoSO itemToCollect;
        [SerializeField] private int amountToComplete = 0;
        [SerializeField] private int amountCollected = 0;

        private void OnEnable()
        {
            QuestManager.instance.collectEvents.onItemCollected += ItemCollected;
        }

        private void OnDisable()
        {
            QuestManager.instance.collectEvents.onItemCollected -= ItemCollected;
        }

        private void ItemCollected(ItemInfoSO itemCollected)
        {
            if (itemCollected == itemToCollect)
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
