using UnityEngine;

namespace RPGToolkit
{
    public class DestroyObjects : MonoBehaviour
    {
        void Start()
        {
            if (PlayerController.instance)
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
}