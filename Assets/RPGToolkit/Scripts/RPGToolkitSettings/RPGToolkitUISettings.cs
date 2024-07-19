using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace RPGToolkit
{
    [ExecuteInEditMode]
    public class RPGToolkitUISettings : MonoBehaviour
    {
        public enum ColorType
        {
            PRIMARY,
            SECONDARY,
            TERTIARY,
            ACCENT,
            BACKGROUND,
            ALT_BACKGROUND
        }

        [Header("Resources")]
        public RPGToolkitManager RPGToolkitAsset;
        public ColorType colorType;
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
            else if (textObject != null)
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
                    else if (textObject != null)
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
                // Colors
                if (useCustomColor == false)
                {
                    if (keepAlphaValue == false)
                    {
                        if (colorType == ColorType.PRIMARY)
                        {
                            uiColor = RPGToolkitAsset.uiPrimaryColor;
                        }
                        else if (colorType == ColorType.SECONDARY)
                        {
                            uiColor = RPGToolkitAsset.uiSecondaryColor;
                        }
                        else if (colorType == ColorType.TERTIARY)
                        {
                            uiColor = RPGToolkitAsset.uiTertiaryColor;
                        }
                        else if (colorType == ColorType.ACCENT)
                        {
                            uiColor = RPGToolkitAsset.uiAccentColor;
                        }
                        else if (colorType == ColorType.BACKGROUND)
                        {
                            uiColor = RPGToolkitAsset.uiBackgroundColor;
                        }
                        else if (colorType == ColorType.ALT_BACKGROUND)
                        {
                            uiColor = RPGToolkitAsset.uiAltBackgroundColor;
                        }
                    }
                    else
                    {
                        if (colorType == ColorType.PRIMARY)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiPrimaryColor.r, RPGToolkitAsset.uiPrimaryColor.g, RPGToolkitAsset.uiPrimaryColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.SECONDARY)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiSecondaryColor.r, RPGToolkitAsset.uiSecondaryColor.g, RPGToolkitAsset.uiSecondaryColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.TERTIARY)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiTertiaryColor.r, RPGToolkitAsset.uiTertiaryColor.g, RPGToolkitAsset.uiTertiaryColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.ACCENT)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiAccentColor.r, RPGToolkitAsset.uiAccentColor.g, RPGToolkitAsset.uiAccentColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.BACKGROUND)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiBackgroundColor.r, RPGToolkitAsset.uiBackgroundColor.g, RPGToolkitAsset.uiBackgroundColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.ALT_BACKGROUND)
                        {
                            uiColor = new Color(RPGToolkitAsset.uiAltBackgroundColor.r, RPGToolkitAsset.uiAltBackgroundColor.g, RPGToolkitAsset.uiAltBackgroundColor.b, uiColor.a);
                        }
                    }
                }

                if (imageObject != null)
                {
                    imageObject.color = uiColor;
                }
                else if (textObject != null)
                {
                    textObject.color = uiColor;
                }

                // Fonts
                // if (fontType == FontType.Light)
                //     textObject.font = UIManagerAsset.lightFont;
                // else if (fontType == FontType.Regular)
                //     textObject.font = UIManagerAsset.regularFont;
                // else if (fontType == FontType.Medium)
                //     textObject.font = UIManagerAsset.mediumFont;
                // else if (fontType == FontType.Semibold)
                //     textObject.font = UIManagerAsset.semiBoldFont;
                // else if (fontType == FontType.Bold)
                //     textObject.font = UIManagerAsset.boldFont;
            }
            catch
            {

            }
        }
    }
}