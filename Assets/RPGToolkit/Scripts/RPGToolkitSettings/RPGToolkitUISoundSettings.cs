using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGToolkit
{
    [ExecuteInEditMode]
    [AddComponentMenu("RPG Toolkit/RPG Toolkit UI Sound Element")]
    public class RPGToolkitUISoundSettings : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;

        [Header("Settings")]
        public bool enableHoverSound = true;
        public bool enableClickSound = true;
        public bool checkForInteraction = true;

        private Button sourceButton;

        private void OnEnable()
        {
            if (RPGToolkitAsset == null)
            {
                try
                {
                    RPGToolkitAsset = AssetDatabase.LoadAssetAtPath<RPGToolkitManager>("Assets/RPGToolkit/RPGToolkitManager.asset");
                }
                catch
                {
                    Debug.Log("RPGToolkitManager not found in Assets/RPGToolkit.", this);
                    this.enabled = false;
                }
            }
        }

        private void Awake()
        {
            if (checkForInteraction == true)
            {
                sourceButton = gameObject.GetComponent<Button>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
            {
                return;
            }

            if (enableHoverSound == true)
            {
                AudioManager.Instance.PlayAudioClip(RPGToolkitAsset.hoverSound, false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
            {
                return;
            }

            if (enableClickSound == true)
            {
                AudioManager.Instance.PlayAudioClip(RPGToolkitAsset.clickSound, false);
            }
        }
    }
}

