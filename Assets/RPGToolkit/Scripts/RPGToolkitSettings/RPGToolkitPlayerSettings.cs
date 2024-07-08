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
                // Inventory
                playerController.hasInventory = RPGToolkitAsset.hasInventory;

                // Level
                playerController.hasLevel = RPGToolkitAsset.hasLevel;
                playerController.currentLevel = RPGToolkitAsset.startingLevel;
                playerController.currentExperience = RPGToolkitAsset.startingExperience;
                playerController.maxExperience = RPGToolkitAsset.maxExperience;

                // Health
                playerController.hasHealthPoint = RPGToolkitAsset.hasHealth;
                playerController.maxHealthPoint = RPGToolkitAsset.maxHealth;
                playerController.currentHealthPoint = playerController.maxHealthPoint;

                // Mana
                playerController.hasManaPoint = RPGToolkitAsset.hasMana;
                playerController.maxManaPoint = RPGToolkitAsset.maxMana;
                playerController.currentManaPoint = playerController.maxManaPoint;

                // Stamina
                playerController.hasStaminaPoint = RPGToolkitAsset.hasStamina;
                playerController.maxStaminaPoint = RPGToolkitAsset.maxStamina;
                playerController.currentStaminaPoint = playerController.maxStaminaPoint;

                // Dash
                playerController.hasDash = RPGToolkitAsset.hasDash;
                playerController.phaseThroughLayers = RPGToolkitAsset.phaseThroughLayers;
                playerController.dodgeableLayers = RPGToolkitAsset.dodgeableLayers;
                playerController.dashPower = RPGToolkitAsset.dashPower;
                playerController.dashDuration = RPGToolkitAsset.dashDuration;
                playerController.dashCooldown = RPGToolkitAsset.dashCooldown;

                // Wall Jump
                playerController.hasWallJump = RPGToolkitAsset.hasWallJump;
                playerController.wallJumpForce = RPGToolkitAsset.wallJumpForce;
                playerController.wallJumpDuration = RPGToolkitAsset.wallJumpDuration;
            }
        }
    }
}