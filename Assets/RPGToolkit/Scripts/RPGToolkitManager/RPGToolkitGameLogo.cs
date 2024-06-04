using UnityEngine;
using UnityEngine.UI;

namespace RPGToolkit
{
    [ExecuteInEditMode]
    public class GameLogo : MonoBehaviour
    {
        [Header("Resources")]
        public RPGToolkitManager RPGManagerAsset;
        public Image logoObject;

        [Header("Settings")]
        public bool keepAlphaValue = false;
        public bool useCustomColor = false;

        bool dynamicUpdateEnabled;

        private void OnEnable()
        {
            if (RPGManagerAsset == null)
            {
                try { RPGManagerAsset = Resources.Load<RPGToolkitManager>("RPG Toolkit Manager"); }
                catch { Debug.Log("No Manager found. Assign it manually, otherwise it won't work properly.", this); }
            }
        }

        private void Awake()
        {
            if (dynamicUpdateEnabled == false)
            {
                this.enabled = true;
                UpdateLogo();
            }

            if (logoObject == null)
                logoObject = gameObject.GetComponent<Image>();
        }

        private void LateUpdate()
        {
            if (RPGManagerAsset != null)
            {
                if (RPGManagerAsset.enableDynamicUpdate == true)
                    dynamicUpdateEnabled = true;
                else
                    dynamicUpdateEnabled = false;

                if (dynamicUpdateEnabled == true)
                    UpdateLogo();
            }
        }

        private void UpdateLogo()
        {
            try
            {
                //logoObject.sprite = RPGManagerAsset.gameLogo;

                if (useCustomColor == false)
                {
                    if (keepAlphaValue == false)
                    {
                        //logoObject.color = RPGManagerAsset.logoColor;
                    }
                    else
                    {
                        //logoObject.color = new Color(RPGManagerAsset.logoColor.r, RPGManagerAsset.logoColor.g, RPGManagerAsset.logoColor.b, logoObject.color.a);
                    }
                }
            }

            catch { }
        }
    }
}
