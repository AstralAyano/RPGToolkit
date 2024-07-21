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

        // Keybinds
        public KeyCode interactKey = KeyCode.F;
        public KeyCode questBookKey = KeyCode.Q;

        // Player Module
        public bool hasInventory = false;
        public bool hasLevel = false;
        public int startingLevel = 0;
        public float startingExperience = 0;
        public float maxExperience = 0;
        public bool hasHealth = false;
        public bool hasHealthUI = false;
        public float maxHealth = 0;
        public bool hasMana = false;
        public bool hasManaUI = false;
        public float maxMana = 0;
        public bool hasStamina = false;
        public bool hasStaminaUI = false;
        public float maxStamina = 0;

        public bool hasDash = false;
        public List<LayerMask> phaseThroughLayers = new List<LayerMask>();
        public List<LayerMask> dodgeableLayers = new List<LayerMask>();
        public float dashPower = 0;
        public float dashDuration = 0;
        public float dashCooldown = 0;
        public bool hasWallJump = false;
        public LayerMask wallLayer;
        public Vector2 wallJumpForce = new Vector2(0, 0);
        public float wallJumpDuration = 0;

        public bool hasAttack;
        public bool hasBlock;

        public float baseWalkSpeed = 0;
        public float baseJumpHeight = 0;
        public float wallSlidingSpeed;
        

        // Quest Module
        public bool hasQuestTrackUI = true;
        public bool hasQuestBookUI = true;
        public bool saveQuest = true;
        public bool loadQuest = true;

        // UI Colors
        public Color uiPrimaryColor = new Color(255, 255, 255, 255);
        public Color uiSecondaryColor = new Color(255, 255, 255, 255);
        public Color uiTertiaryColor = new Color(255, 255, 255, 255);
        public Color uiAccentColor = new Color(255, 255, 255, 255);
        public Color uiBackgroundColor = new Color(255, 255, 255, 255);
        public Color uiAltBackgroundColor = new Color(255, 255, 255, 255);

        // Fonts
        public TMP_FontAsset lightFont;
        public TMP_FontAsset regularFont;
        public TMP_FontAsset mediumFont;
        public TMP_FontAsset semiBoldFont;
        public TMP_FontAsset boldFont;

        // Font Colors
        public Color fontPrimaryColor = new Color(255, 255, 255, 255);
        public Color fontSecondaryColor = new Color(255, 255, 255, 255);
        public Color fontTertiaryColor = new Color(255, 255, 255, 255);
        public Color fontAccentColor = new Color(255, 255, 255, 255);

        // UI Sounds
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