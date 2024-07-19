using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace RPGToolkit
{
    [ExecuteInEditMode]
    public class RPGToolkitUISettings : MonoBehaviour
    {
        public enum UIColorType
        {
            PRIMARY,
            SECONDARY,
            TERTIARY,
            ACCENT,
            BACKGROUND,
            ALT_BACKGROUND
        }

        public enum FontType
        {
            LIGHT,
            REGULAR,
            MEDIUM,
            SEMIBOLD,
            BOLD
        }

        public enum FontColorType
        {
            PRIMARY,
            SECONDARY,
            TERTIARY,
            ACCENT
        }

        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;
        public UIColorType uiColorType;
        public FontType fontType;
        public FontColorType fontColorType;
        private Image imageObject;
        private TextMeshProUGUI textObject;
        
        [Header("Settings")]
        public bool keepAlphaValue = false;
        public bool useCustomColor = false;

        bool dynamicUpdateEnabled;

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
            if (dynamicUpdateEnabled == false)
            {
                this.enabled = true;
            }

            if (imageObject == null && textObject == null)
            {
                imageObject = gameObject.GetComponent<Image>();
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
            }

            if (imageObject != null)
            {
                UpdateUI(imageObject.color);
            }
            
            if (textObject != null)
            {
                UpdateUI(textObject.color);
            }
        }

        private void LateUpdate()
        {
            if (RPGToolkitAsset != null)
            {
                if (RPGToolkitAsset.enableDynamicUpdate == true)
                {
                    dynamicUpdateEnabled = true;
                }
                else
                {
                    dynamicUpdateEnabled = false;
                }

                if (dynamicUpdateEnabled == true)
                {
                    if (imageObject != null)
                    {
                        UpdateUI(imageObject.color);
                    }
                    
                    if (textObject != null)
                    {
                        UpdateUI(textObject.color);
                    }
                }
            }
        }

        private void UpdateUI(Color uiColor)
        {
            try
            {
                if (imageObject != null && !useCustomColor)
                {
                    if (!keepAlphaValue)
                    {
                        switch (uiColorType)
                        {
                            case UIColorType.PRIMARY:
                                uiColor = RPGToolkitAsset.uiPrimaryColor;
                                break;
                            case UIColorType.SECONDARY:
                                uiColor = RPGToolkitAsset.uiSecondaryColor;
                                break;
                            case UIColorType.TERTIARY:
                                uiColor = RPGToolkitAsset.uiTertiaryColor;
                                break;
                            case UIColorType.ACCENT:
                                uiColor = RPGToolkitAsset.uiAccentColor;
                                break;
                            case UIColorType.BACKGROUND:
                                uiColor = RPGToolkitAsset.uiBackgroundColor;
                                break;
                            case UIColorType.ALT_BACKGROUND:
                                uiColor = RPGToolkitAsset.uiAltBackgroundColor;
                                break;
                        }
                    }
                    else
                    {
                        switch (uiColorType)
                        {
                            case UIColorType.PRIMARY:
                                uiColor = new Color(RPGToolkitAsset.uiPrimaryColor.r, RPGToolkitAsset.uiPrimaryColor.g, RPGToolkitAsset.uiPrimaryColor.b, uiColor.a);
                                break;
                            case UIColorType.SECONDARY:
                                uiColor = new Color(RPGToolkitAsset.uiSecondaryColor.r, RPGToolkitAsset.uiSecondaryColor.g, RPGToolkitAsset.uiSecondaryColor.b, uiColor.a);
                                break;
                            case UIColorType.TERTIARY:
                                uiColor = new Color(RPGToolkitAsset.uiTertiaryColor.r, RPGToolkitAsset.uiTertiaryColor.g, RPGToolkitAsset.uiTertiaryColor.b, uiColor.a);
                                break;
                            case UIColorType.ACCENT:
                                uiColor = new Color(RPGToolkitAsset.uiAccentColor.r, RPGToolkitAsset.uiAccentColor.g, RPGToolkitAsset.uiAccentColor.b, uiColor.a);
                                break;
                            case UIColorType.BACKGROUND:
                                uiColor = new Color(RPGToolkitAsset.uiBackgroundColor.r, RPGToolkitAsset.uiBackgroundColor.g, RPGToolkitAsset.uiBackgroundColor.b, uiColor.a);
                                break;
                            case UIColorType.ALT_BACKGROUND:
                                uiColor = new Color(RPGToolkitAsset.uiAltBackgroundColor.r, RPGToolkitAsset.uiAltBackgroundColor.g, RPGToolkitAsset.uiAltBackgroundColor.b, uiColor.a);
                                break;
                        }
                    }
                }
                
                if (textObject != null)
                {
                    // Fonts
                    if (fontType == FontType.LIGHT)
                    {
                        textObject.font = RPGToolkitAsset.lightFont;
                    }
                    else if (fontType == FontType.REGULAR)
                    {
                        textObject.font = RPGToolkitAsset.regularFont;
                    }
                    else if (fontType == FontType.MEDIUM)
                    {
                        textObject.font = RPGToolkitAsset.mediumFont;
                    }
                    else if (fontType == FontType.SEMIBOLD)
                    {
                        textObject.font = RPGToolkitAsset.semiBoldFont;
                    }
                    else if (fontType == FontType.BOLD)
                    {
                        textObject.font = RPGToolkitAsset.boldFont;
                    }

                    if (!useCustomColor)
                    {
                        if (!keepAlphaValue)
                        {
                            switch (fontColorType)
                            {
                                case FontColorType.PRIMARY:
                                    uiColor = RPGToolkitAsset.fontPrimaryColor;
                                    break;
                                case FontColorType.SECONDARY:
                                    uiColor = RPGToolkitAsset.fontSecondaryColor;
                                    break;
                                case FontColorType.TERTIARY:
                                    uiColor = RPGToolkitAsset.fontTertiaryColor;
                                    break;
                                case FontColorType.ACCENT:
                                    uiColor = RPGToolkitAsset.fontAccentColor;
                                    break;
                            }
                        }
                        else
                        {
                            switch (fontColorType)
                            {
                                case FontColorType.PRIMARY:
                                    uiColor = new Color(RPGToolkitAsset.fontPrimaryColor.r, RPGToolkitAsset.fontPrimaryColor.g, RPGToolkitAsset.fontPrimaryColor.b, uiColor.a);
                                    break;
                                case FontColorType.SECONDARY:
                                    uiColor = new Color(RPGToolkitAsset.fontSecondaryColor.r, RPGToolkitAsset.fontSecondaryColor.g, RPGToolkitAsset.fontSecondaryColor.b, uiColor.a);
                                    break;
                                case FontColorType.TERTIARY:
                                    uiColor = new Color(RPGToolkitAsset.fontTertiaryColor.r, RPGToolkitAsset.fontTertiaryColor.g, RPGToolkitAsset.fontTertiaryColor.b, uiColor.a);
                                    break;
                                case FontColorType.ACCENT:
                                    uiColor = new Color(RPGToolkitAsset.fontAccentColor.r, RPGToolkitAsset.fontAccentColor.g, RPGToolkitAsset.fontAccentColor.b, uiColor.a);
                                    break;
                            }
                        }
                    }
                }

                if (imageObject != null)
                {
                    imageObject.color = uiColor;
                }
                
                if (textObject != null)
                {
                    textObject.color = uiColor;
                }
            }
            catch
            {

            }
        }
    }
}