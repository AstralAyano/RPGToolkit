using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Object")]
    public InventoryManager invManager;

    [Header("List of Scriptable Objects")]
    public Item[] itemsToPickup;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        
        if (other.gameObject.name.Contains("Major Health Potion") && invManager.AddItem(itemsToPickup[0]))
        {
            //gameObject.GetComponentInParent<PlayerController>().PlaySFX("Health");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name.Contains("Minor Mana Potion") && invManager.AddItem(itemsToPickup[1]))
        {
            //gameObject.GetComponentInParent<PlayerController>().PlaySFX("Mana");
            Destroy(other.gameObject);
        }
    }
}
