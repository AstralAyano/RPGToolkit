using UnityEngine;

namespace RPGToolkit
{
    public class PickUpItem : MonoBehaviour
    {
        [Header("Object")]
        public InventoryManager invManager;

        [Header("List of Scriptable Objects")]
        public ItemInfoSO[] itemsToPickup;

        private void Start()
        {
            if (PlayerController.instance.hasInventory)
            {
                invManager = GameObject.FindWithTag("RPGToolkitInventory").GetComponent<InventoryManager>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            int itemIndex = 0;

            if (!PlayerController.instance.hasInventory)
            {
                return;
            }
            
            if (other.gameObject.name.Contains("Health Potion") && invManager.AddItem(itemsToPickup[0]))
            {
                //gameObject.GetComponentInParent<PlayerController>().PlaySFX("Health");
                itemIndex = 0;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.name.Contains("Mana Potion") && invManager.AddItem(itemsToPickup[1]))
            {
                //gameObject.GetComponentInParent<PlayerController>().PlaySFX("Mana");
                itemIndex = 1;
                Destroy(other.gameObject);
            }

            QuestManager.instance.collectEvents.ItemCollected(itemsToPickup[itemIndex]);
        }
    }
}