using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

namespace mainMenuSystem
{
    public class MenuController : MonoBehaviour
    {
        #region Default Values
        [Header("Default Settings")]
        [SerializeField] public int defaultResolution;
        [SerializeField] private bool defaultFullscreen = true;
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private float currentVolume;

        [Header("Scene to Load")]
        public string startGameButtonScene;

        private int menuNumber;
        #endregion

        #region Menu Dialogs
        [Header("Menu")]
        [SerializeField] private SceneLoadManager loadManager;
        [SerializeField] private GameObject[] gameTitle;
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private GameObject optionsCanvas;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject creditsMenu;
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject confirmationMenu;
        [Space(10)]
        [Header("Menu Popout Dialogs")]
        [SerializeField] private GameObject startGameDialog;
        #endregion

        #region Slider Linking
        [Header("Menu Sliders")]
        [SerializeField] private Resolution[] resolutions;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private List<Resolution> filteredResolutions;
        [Space(10)]
        [SerializeField] private Toggle fullscreenToggle;
        [Space(10)]
        [SerializeField] private TMP_Text volumeText;
        [SerializeField] private Slider volumeSlider;
        #endregion

        #region Initialisation - Button Selection & Menu Order
        private void Awake()
        {
            menuNumber = 1;

            currentVolume = defaultVolume;

            int currResolutionIndex = 0;
            float currRefreshRate;

            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();

            resolutionDropdown.ClearOptions();
            currRefreshRate = Screen.currentResolution.refreshRate;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRate == currRefreshRate)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }

            List<string> options = new List<string>();

            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " [" + filteredResolutions[i].refreshRate + " Hz]";
                options.Add(resolutionOption);

                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("resolutionIndex");
            resolutionDropdown.RefreshShownValue();

            defaultResolution = PlayerPrefs.GetInt("resolutionIndex");
            
            loadManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoadManager>();
        }
        #endregion

        // MAIN SECTION
        public IEnumerator ConfirmationBox()
        {
            confirmationMenu.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuNumber == 2 || menuNumber == 6 || menuNumber == 7 || menuNumber == 8)
                {
                    GoBackToMainMenu();
                    ClickSound();
                }

                else if (menuNumber == 3 || menuNumber == 4 || menuNumber == 5)
                {
                    GoBackToOptionsMenu();
                    ClickSound();
                }
            }
        }

        private void ClickSound()
        {
            GetComponent<AudioSource>().Play();
        }

        #region Menu Mouse Clicks
        public void MouseClick(string buttonPressed)
        {
            if (buttonPressed == "Graphics")
            {
                for (int i = 0; i < 4; i++)
                {
                    gameTitle[i].SetActive(false);
                }

                optionsCanvas.SetActive(false);
                graphicsMenu.SetActive(true);
                menuNumber = 3;
            }

            if (buttonPressed == "Sound")
            {
                for (int i = 0; i < 4; i++)
                {
                    gameTitle[i].SetActive(false);
                }

                optionsCanvas.SetActive(false);
                soundMenu.SetActive(true);
                menuNumber = 4;
            }

            if (buttonPressed == "Exit")
            {
                Debug.Log("Quit Successful");
                Application.Quit();
            }

            if (buttonPressed == "Options")
            {
                menuCanvas.SetActive(false);
                optionsCanvas.SetActive(true);
                menuNumber = 2;
            }

            if (buttonPressed == "StartGame")
            {
                for (int i = 0; i < 4; i++)
                {
                    gameTitle[i].SetActive(false);
                }

                menuCanvas.SetActive(false);
                startGameDialog.SetActive(true);
                menuNumber = 6;
            }

            if (buttonPressed == "Controls")
            {
                controlsMenu.SetActive(true);
                menuNumber = 7;
            }

            if (buttonPressed == "Credits")
            {
                creditsMenu.SetActive(true);
                menuNumber = 8;
            }
        }
        #endregion

        public void VolumeSlider(float volume)
        {
            AudioListener.volume = volume;
            volumeText.text = $"{volume * 100:0}";
        }

        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            Debug.Log(PlayerPrefs.GetFloat("masterVolume"));

            StartCoroutine(ConfirmationBox());

            currentVolume = PlayerPrefs.GetFloat("masterVolume");
        }

        public void ResolutionApply(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];

            PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
            PlayerPrefs.SetInt("resolutionWidth", resolution.width);
            PlayerPrefs.SetInt("resolutionHeight", resolution.height);
            Debug.Log(PlayerPrefs.GetInt("resolutionIndex"));

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Debug.Log("Resolution Applied");
        }

        public void FullscreenApply(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            Debug.Log("Fullscreen Applied");
        }

        #region ResetButton
        public void ResetButton(string GraphicsMenu)
        {
            if (GraphicsMenu == "Graphics")
            {
                Resolution resolution = filteredResolutions[defaultResolution];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

                resolutionDropdown.value = defaultResolution;
                resolutionDropdown.RefreshShownValue();

                fullscreenToggle.isOn = defaultFullscreen;

                Debug.Log($"Reset Applied : {resolution.width} x {resolution.height} : {fullscreenToggle.isOn}");
            }

            if (GraphicsMenu == "Audio")
            {
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumeText.text = $"{defaultVolume * 100:0}";
                VolumeApply();
            }
        }
        #endregion

        #region BackButton
        public void BackButton()
        {
            currentVolume = PlayerPrefs.GetFloat("masterVolume");

            AudioListener.volume = currentVolume;
            volumeSlider.value = currentVolume;
            volumeText.text = $"{currentVolume * 100:0}";
            VolumeApply();
        }
        #endregion

        #region Dialog Options
        public void ClickNewGameDialog(string ButtonType)
        {
            if (ButtonType == "Yes")
            {
                loadManager.LoadScene(startGameButtonScene);
            }

            if (ButtonType == "No")
            {
                GoBackToMainMenu();
            }
        }
        #endregion

        #region Back to Menus
        public void GoBackToMainMenu()
        {
            for (int i = 0; i < 4; i++)
            {
                gameTitle[i].SetActive(true);
            }

            menuCanvas.SetActive(true);
            startGameDialog.SetActive(false);
            optionsCanvas.SetActive(false);
            controlsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);

            menuNumber = 1;
        }

        public void GoBackToOptionsMenu()
        {
            for (int i = 0; i < 4; i++)
            {
                gameTitle[i].SetActive(true);
            }

            optionsCanvas.SetActive(true);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);

            BackButton();

            menuNumber = 2;
        }

        public void ClickQuitOptions()
        {
            GoBackToMainMenu();
        }
        #endregion
    }
}
