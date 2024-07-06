using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPGToolkit
{
    [CreateAssetMenu(fileName = "RPGToolkitManager", menuName = "RPGToolkit/New RPGToolkit Manager", order = 11)]
    public class RPGToolkitManager : ScriptableObject
    {
        [HideInInspector] public bool enableDynamicUpdate = true;
        [HideInInspector] public bool enableExtendedColorPicker = true;
        [HideInInspector] public bool editorHints = true;

        //[Header("PLAYER MODULE")]
        public bool hasInventory = false;
        public bool hasLevel = false;
        public int startingLevel = 0;
        public float startingExperience = 0;
        public float maxExperience = 0;
        public bool hasHealth = false;
        public float maxHealth = 0;
        public bool hasMana = false;
        public float maxMana = 0;
        public bool hasStamina = false;
        public float maxStamina = 0;

        //[Header("QUEST MODULE")]
        public bool hasQuestTrackUI = true;
        public bool hasQuestBookUI = true;
        public bool saveQuest = true;
        public bool loadQuest = true;

        //[Header("UI COLORS")]
        public Color primaryColor = new Color(255, 255, 255, 255);
        public Color secondaryColor = new Color(255, 255, 255, 255);
        public Color primaryReversed = new Color(255, 255, 255, 255);
        public Color negativeColor = new Color(255, 255, 255, 255);
        public Color backgroundColor = new Color(255, 255, 255, 255);

        //[Header("FONTS")]
        public TMP_FontAsset lightFont;
        public TMP_FontAsset regularFont;
        public TMP_FontAsset mediumFont;
        public TMP_FontAsset semiBoldFont;
        public TMP_FontAsset boldFont;

        //[Header("SOUNDS")]
        public AudioClip backgroundMusic;
        public AudioClip hoverSound;
        public AudioClip clickSound;

        public enum ButtonThemeType
        {
            BASIC,
            CUSTOM
        }

        public enum BackgroundType
        {
            BASIC,
            ADVANCED
        }
    }
}