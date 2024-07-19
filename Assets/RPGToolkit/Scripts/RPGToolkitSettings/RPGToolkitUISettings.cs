using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;

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
                        uiColor = uiColorType switch
                        {
                            UIColorType.PRIMARY => RPGToolkitAsset.uiPrimaryColor,
                            UIColorType.SECONDARY => RPGToolkitAsset.uiSecondaryColor,
                            UIColorType.TERTIARY => RPGToolkitAsset.uiTertiaryColor,
                            UIColorType.ACCENT => RPGToolkitAsset.uiAccentColor,
                            UIColorType.BACKGROUND => RPGToolkitAsset.uiBackgroundColor,
                            UIColorType.ALT_BACKGROUND => RPGToolkitAsset.uiAltBackgroundColor,
                            _ => uiColor
                        };
                    }
                    else
                    {
                        uiColor = uiColorType switch
                        {
                            UIColorType.PRIMARY => new Color(RPGToolkitAsset.uiPrimaryColor.r, RPGToolkitAsset.uiPrimaryColor.g, RPGToolkitAsset.uiPrimaryColor.b, uiColor.a),
                            UIColorType.SECONDARY => new Color(RPGToolkitAsset.uiSecondaryColor.r, RPGToolkitAsset.uiSecondaryColor.g, RPGToolkitAsset.uiSecondaryColor.b, uiColor.a),
                            UIColorType.TERTIARY => new Color(RPGToolkitAsset.uiTertiaryColor.r, RPGToolkitAsset.uiTertiaryColor.g, RPGToolkitAsset.uiTertiaryColor.b, uiColor.a),
                            UIColorType.ACCENT => new Color(RPGToolkitAsset.uiAccentColor.r, RPGToolkitAsset.uiAccentColor.g, RPGToolkitAsset.uiAccentColor.b, uiColor.a),
                            UIColorType.BACKGROUND => new Color(RPGToolkitAsset.uiBackgroundColor.r, RPGToolkitAsset.uiBackgroundColor.g, RPGToolkitAsset.uiBackgroundColor.b, uiColor.a),
                            UIColorType.ALT_BACKGROUND => new Color(RPGToolkitAsset.uiAltBackgroundColor.r, RPGToolkitAsset.uiAltBackgroundColor.g, RPGToolkitAsset.uiAltBackgroundColor.b, uiColor.a),
                            _ => uiColor
                        };
                    }
                }
                
                if (textObject != null)
                {
                    // Fonts
                    textObject.font = fontType switch
                    {
                        FontType.LIGHT => RPGToolkitAsset.lightFont,
                        FontType.REGULAR => RPGToolkitAsset.regularFont,
                        FontType.MEDIUM => RPGToolkitAsset.mediumFont,
                        FontType.SEMIBOLD => RPGToolkitAsset.semiBoldFont,
                        FontType.BOLD => RPGToolkitAsset.boldFont,
                        _ => textObject.font
                    };

                    if (!useCustomColor)
                    {
                        if (!keepAlphaValue)
                        {
                            uiColor = fontColorType switch
                            {
                                FontColorType.PRIMARY => RPGToolkitAsset.fontPrimaryColor,
                                FontColorType.SECONDARY => RPGToolkitAsset.fontSecondaryColor,
                                FontColorType.TERTIARY => RPGToolkitAsset.fontTertiaryColor,
                                FontColorType.ACCENT => RPGToolkitAsset.fontAccentColor,
                                _ => uiColor
                            };
                        }
                        else
                        {
                            uiColor = fontColorType switch
                            {
                                FontColorType.PRIMARY => new Color(RPGToolkitAsset.fontPrimaryColor.r, RPGToolkitAsset.fontPrimaryColor.g, RPGToolkitAsset.fontPrimaryColor.b, uiColor.a),
                                FontColorType.SECONDARY => new Color(RPGToolkitAsset.fontSecondaryColor.r, RPGToolkitAsset.fontSecondaryColor.g, RPGToolkitAsset.fontSecondaryColor.b, uiColor.a),
                                FontColorType.TERTIARY => new Color(RPGToolkitAsset.fontTertiaryColor.r, RPGToolkitAsset.fontTertiaryColor.g, RPGToolkitAsset.fontTertiaryColor.b, uiColor.a),
                                FontColorType.ACCENT => new Color(RPGToolkitAsset.fontAccentColor.r, RPGToolkitAsset.fontAccentColor.g, RPGToolkitAsset.fontAccentColor.b, uiColor.a),
                                _ => uiColor
                            };
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