using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    void Start()
    {
        if (PlayerControllerA.instance)
        {
            Destroy(PlayerControllerA.instance.gameObject);
        }
        
        if (InventoryManager.instance)
        {
            Destroy(InventoryManager.instance.gameObject);
        }

        if (PauseMenu.instance)
        {
            Destroy(PauseMenu.instance.gameObject);
        }
    }
}
