using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class PickUpItem : MonoBehaviour
    {
        [Header("Object")]
        public InventoryManager invManager;

        [Header("List of Scriptable Objects")]
        private Dictionary<string, ItemInfoSO> itemDictionary;

        private void Start()
        {
            if (PlayerController.instance.hasInventory)
            {
                invManager = GameObject.FindWithTag("RPGToolkitInventory").GetComponent<InventoryManager>();

                // Load all ItemInfoSO from Resources and create a dictionary
                LoadItemInfo();
            }
        }

        private void LoadItemInfo()
        {
            // Load all ItemInfoSO from the specified path
            ItemInfoSO[] allItems = Resources.LoadAll<ItemInfoSO>("RPGToolkit/Items");
            itemDictionary = new Dictionary<string, ItemInfoSO>();

            foreach (ItemInfoSO item in allItems)
            {
                if (!itemDictionary.ContainsKey(item.itemName.ToLower()))
                {
                    itemDictionary.Add(item.itemName.ToLower(), item);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!PlayerController.instance.hasInventory)
            {
                return;
            }

            string itemName = other.gameObject.name.ToLower();

            try
            {
                if (itemDictionary.TryGetValue(itemName, out ItemInfoSO itemInfo))
                {
                    if (invManager.AddItem(itemInfo))
                    {
                        Destroy(other.gameObject);
                        CallCollectEvents(itemInfo);
                    }
                }
            }
            catch
            {
                
            }
        }

        private void CallCollectEvents(ItemInfoSO itemInfo)
        {
            EventsManager.Instance.collectEvents.ItemCollected(itemInfo);
        }
    }
}