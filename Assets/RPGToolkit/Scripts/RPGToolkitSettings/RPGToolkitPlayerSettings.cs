using System.Collections.Generic;
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
                EventsManager.Instance.playerEvents.PlayerLevelChange(playerController.currentLevel);
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

                // Ensure phaseThroughLayers has the correct size before assigning values
                if (playerController.phaseThroughLayers.Count != RPGToolkitAsset.phaseThroughLayers.Count)
                {
                    playerController.phaseThroughLayers = new List<LayerMask>(new LayerMask[RPGToolkitAsset.phaseThroughLayers.Count]);
                }
                // Clear the list and add new elements to avoid resizing issues
                playerController.phaseThroughLayers.Clear();
                foreach (var layer in RPGToolkitAsset.phaseThroughLayers)
                {
                    playerController.phaseThroughLayers.Add((LayerMask)layer);
                }
                
                // Ensure dodgeableLayers has the correct size before assigning values
                if (playerController.dodgeableLayers.Count != RPGToolkitAsset.dodgeableLayers.Count)
                {
                    playerController.dodgeableLayers = new List<LayerMask>(new LayerMask[RPGToolkitAsset.dodgeableLayers.Count]);
                }
                // Clear the list and add new elements to avoid resizing issues
                playerController.dodgeableLayers.Clear();
                foreach (var layer in RPGToolkitAsset.dodgeableLayers)
                {
                    playerController.dodgeableLayers.Add((LayerMask)layer);
                }

                playerController.dashPower = RPGToolkitAsset.dashPower;
                playerController.dashDuration = RPGToolkitAsset.dashDuration;
                playerController.dashCooldown = RPGToolkitAsset.dashCooldown;

                // Wall Jump
                playerController.wallLayer = RPGToolkitAsset.wallLayer;
                playerController.hasWallJump = RPGToolkitAsset.hasWallJump;
                playerController.wallJumpForce = RPGToolkitAsset.wallJumpForce;
                playerController.wallJumpDuration = RPGToolkitAsset.wallJumpDuration;
            }
        }
    }
}