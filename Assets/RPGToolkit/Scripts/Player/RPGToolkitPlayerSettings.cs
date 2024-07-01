using System.Collections;
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

            playerController.hasInventory = RPGToolkitAsset.hasInventory;
            playerController.hasQuestBook = RPGToolkitAsset.hasQuestBook;
            playerController.hasLevel = RPGToolkitAsset.hasLevel;
        }
    }
}