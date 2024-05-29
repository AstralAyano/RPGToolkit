using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace mainMenuSystem
{
    public class LoadPreferencesScript : MonoBehaviour
    {
        #region Variables
        // VOLUME
        [Space(20)]
        [SerializeField] private TMP_Text volumeText;
        [SerializeField] private Slider volumeSlider;

        [Space(20)]
        [SerializeField] private bool canUse = false;
        [SerializeField] private MenuController menuController;

        // RESOLUTION
        [Space(20)]
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        // FULLSCREEN
        [Space(20)]
        private bool localFullscreenBool;
        #endregion

        private void Start()
        {
            if (canUse)
            {
                // VOLUME
                if (PlayerPrefs.HasKey("masterVolume"))
                {
                    float localVolume = PlayerPrefs.GetFloat("masterVolume");

                    volumeText.text = $"{localVolume * 100:0}";
                    volumeSlider.value = localVolume;
                    AudioListener.volume = localVolume;

                    Debug.Log("Loaded Player Prefs.");
                }
                else
                {
                    menuController.ResetButton("Audio");
                }

                // RESOLUTION
                if (PlayerPrefs.HasKey("resolutionIndex"))
                {
                    int localResIndex = PlayerPrefs.GetInt("resolutionIndex");
                    int localResWidth = PlayerPrefs.GetInt("resolutionWidth");
                    int localResHeight = PlayerPrefs.GetInt("resolutionHeight");

                    Screen.SetResolution(localResWidth, localResHeight, Screen.fullScreen);

                    resolutionDropdown.value = localResIndex;
                    resolutionDropdown.RefreshShownValue();
                }
                else
                {
                    menuController.ResetButton("Graphics");
                }

                // FULLSCREEN
                if (PlayerPrefs.HasKey("fullscreen"))
                {
                    string localFullscreen = PlayerPrefs.GetString("fullscreen");

                    if (localFullscreen.ToLower() == "true")
                    {
                        localFullscreenBool = true;
                    }
                    else if (localFullscreen.ToLower() == "false")
                    {
                        localFullscreenBool = false;
                    }

                    Screen.fullScreen = localFullscreenBool;
                }
                else
                {
                    Screen.fullScreen = true;
                }
            }
        }
    }
}
