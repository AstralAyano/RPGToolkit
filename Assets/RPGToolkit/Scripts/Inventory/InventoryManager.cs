using UnityEngine;

namespace RPGToolkit
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Objects")]
        public InventorySlot[] invSlots;
        public GameObject invItemPrefab;
        public int selectedSlot = -1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.inputString != null)
            {
                bool isNumber = int.TryParse(Input.inputString, out int number);

                if (isNumber && number > 0 && number < 10)
                {
                    ChangeSlot(number - 1);
                }
            }
        }

        private void ChangeSlot(int newSlot)
        {
            selectedSlot = newSlot;
            Debug.Log("Selected slot " + selectedSlot);
        }

        public bool AddItem(ItemInfoSO item)
        {
            for (int i = 0; i < invSlots.Length; i++)
            {
                InventorySlot slot = invSlots[i];
                InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemSlot != null && itemSlot.item.stackable == true && itemSlot.item == item && itemSlot.count < 5)
                {
                    itemSlot.count++;
                    itemSlot.UpdateCount();
                    return true;
                }
            }

            for (int i = 0; i < invSlots.Length; i++)
            {
                InventorySlot slot = invSlots[i];

                if (slot.GetComponentInChildren<InventoryItem>() == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItem(ItemInfoSO item, int amount)
        {
            int remainingAmount = amount;

            for (int i = 0; i < invSlots.Length && remainingAmount > 0; i++)
            {
                InventorySlot slot = invSlots[i];
                InventoryItem[] itemsInSlot = slot.GetComponentsInChildren<InventoryItem>();

                foreach (InventoryItem itemSlot in itemsInSlot)
                {
                    if (itemSlot.item == item)
                    {
                        if (item.stackable)
                        {
                            // Handle stackable items
                            itemSlot.count -= remainingAmount;

                            if (itemSlot.count <= 0)
                            {
                                Destroy(itemSlot.gameObject);
                                remainingAmount = -itemSlot.count;
                            }
                            else
                            {
                                itemSlot.UpdateCount();
                                remainingAmount = 0;
                            }
                        }
                        else
                        {
                            // Handle non-stackable items
                            Destroy(itemSlot.gameObject);
                            remainingAmount--;
                        }

                        if (remainingAmount <= 0)
                        {
                            return;
                        }
                    }
                }
            }

            if (remainingAmount > 0)
            {
                Debug.LogWarning("Not enough items in inventory to remove.");
            }
        }

        private void SpawnNewItem(ItemInfoSO item, InventorySlot slot)
        {
            GameObject newItemGO = Instantiate(invItemPrefab, slot.transform);

            InventoryItem invItem = newItemGO.GetComponent<InventoryItem>();

            invItem.InitialiseItem(item);
        }

        public ItemInfoSO GetSelectedItem(bool consumable, bool isHealthMax, bool isManaMax)
        {
            InventorySlot slot = invSlots[selectedSlot];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemSlot != null)
            {
                ItemInfoSO item = itemSlot.item;

                if (consumable)
                {
                    if (item.name.Contains("Health Potion") && isHealthMax)
                    {
                        Debug.Log("Health is full.");
                        //sysText.DisplayText("A Scroll of Swift is already in effect.");
                    }
                    else if (item.name.Contains("Mana Potion") && isManaMax)
                    {
                        Debug.Log("Mana is full.");
                        //sysText.DisplayText("You can't use this scroll here.");
                    }
                    else
                    {
                        itemSlot.count--;

                        if (itemSlot.count <= 0)
                        {
                            Destroy(itemSlot.gameObject);
                        }
                        else
                        {
                            itemSlot.UpdateCount();
                        }
                    }
                }

                return item;
            }

            return null;
        }
    }
}