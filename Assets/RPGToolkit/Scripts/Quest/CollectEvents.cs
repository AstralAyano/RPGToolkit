using System;

namespace RPGToolkit
{
    public class CollectEvents
    {
        public event Action<ItemInfoSO> onItemCollected;

        public void ItemCollected(ItemInfoSO itemCollected)
        {
            if (onItemCollected != null)
            {
                onItemCollected(itemCollected);
            }
        }
    }
}
