using UnityEngine;

namespace RPGToolkit
{
    public class RPGToolkitPlayerSettings : MonoBehaviour
    {
        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.hasInventory = RPGToolkitAsset.hasInventory;
                playerController.hasLevel = RPGToolkitAsset.hasLevel;
            }
        }
    }
}