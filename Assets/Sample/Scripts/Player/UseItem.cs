using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [Header("Objects")]
    public InventoryManager invManager;
    [HideInInspector] public PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (PlayerController.instance.hasInventory)
        {
            invManager = GameObject.FindWithTag("InventoryModule").GetComponent<InventoryManager>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSelectedItem();
        }
    }

    public void UseSelectedItem()
    {
        Item usingItem = invManager.GetSelectedItem(true, controller.isHealthMax, controller.isManaMax);

        if (usingItem != null)
        {
            Debug.Log("Used " + usingItem);

            if (usingItem.name.Contains("Major Health Potion") && !controller.isHealthMax)
            {
                controller.RegainStats("HP", 20);
            }
            else if (usingItem.name.Contains("Minor Mana Potion") && !controller.isManaMax)
            {
                controller.RegainStats("MP", 10);
            }
        }
    }
}
