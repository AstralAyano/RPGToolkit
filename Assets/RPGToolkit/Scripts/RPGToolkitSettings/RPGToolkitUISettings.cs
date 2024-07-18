using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEditor.SceneManagement;

namespace RPGToolkit
{
    public class RPGToolkitUISettings : MonoBehaviour
    {
        public enum ColorType
        {
            PRIMARY,
            SECONDARY,
            PRIMARY_REVERSED,
            NEGATIVE,
            BACKGROUND
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

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            if (imageObject != null)
            {
                EditorUtility.SetDirty(imageObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(imageObject);
            }

            if (textObject != null)
            {
                EditorUtility.SetDirty(textObject);
                PrefabUtility.RecordPrefabInstancePropertyModifications(textObject);
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
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
                            uiColor = RPGToolkitAsset.primaryColor;
                        }
                        else if (colorType == ColorType.SECONDARY)
                        {
                            uiColor = RPGToolkitAsset.secondaryColor;
                        }
                        else if (colorType == ColorType.PRIMARY_REVERSED)
                        {
                            uiColor = RPGToolkitAsset.primaryReversed;
                        }
                        else if (colorType == ColorType.NEGATIVE)
                        {
                            uiColor = RPGToolkitAsset.negativeColor;
                        }
                        else if (colorType == ColorType.BACKGROUND)
                        {
                            uiColor = RPGToolkitAsset.backgroundColor;
                        }
                    }
                    else
                    {
                        if (colorType == ColorType.PRIMARY)
                        {
                            uiColor = new Color(RPGToolkitAsset.primaryColor.r, RPGToolkitAsset.primaryColor.g, RPGToolkitAsset.primaryColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.SECONDARY)
                        {
                            uiColor = new Color(RPGToolkitAsset.secondaryColor.r, RPGToolkitAsset.secondaryColor.g, RPGToolkitAsset.secondaryColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.PRIMARY_REVERSED)
                        {
                            uiColor = new Color(RPGToolkitAsset.primaryReversed.r, RPGToolkitAsset.primaryReversed.g, RPGToolkitAsset.primaryReversed.b, uiColor.a);
                        }
                        else if (colorType == ColorType.NEGATIVE)
                        {
                            uiColor = new Color(RPGToolkitAsset.negativeColor.r, RPGToolkitAsset.negativeColor.g, RPGToolkitAsset.negativeColor.b, uiColor.a);
                        }
                        else if (colorType == ColorType.BACKGROUND)
                        {
                            uiColor = new Color(RPGToolkitAsset.backgroundColor.r, RPGToolkitAsset.backgroundColor.g, RPGToolkitAsset.backgroundColor.b, uiColor.a);
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