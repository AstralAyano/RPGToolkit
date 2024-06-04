using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static GameObject instance;

    [SerializeField] private SceneLoadManager sceneLoadManager;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private CanvasGroup pauseMenuGroup;

    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private float fadeSpeed;

    public static bool GameIsPaused = false;

    private bool uiFadeIn = false, uiFadeOut = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        pauseMenuGroup.alpha = 0f;

        //sceneLoadManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneLoadManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (uiFadeIn)
        {
            if (pauseMenuGroup.alpha < 1.0f)
            {
                pauseMenuGroup.alpha += Time.deltaTime * fadeSpeed;

                if (pauseMenuGroup.alpha >= 1.0f)
                {
                    uiFadeIn = false;
                    Time.timeScale = 0f;
                    resumeBtn.interactable = true;
                    menuBtn.interactable = true;
                    exitBtn.interactable = true;
                }
            }
        }

        if (uiFadeOut)
        {
            if (pauseMenuGroup.alpha >= 0f)
            {
                pauseMenuGroup.alpha -= Time.deltaTime * fadeSpeed;

                if (pauseMenuGroup.alpha == 0f)
                {
                    uiFadeOut = false;
                    pauseMenu.SetActive(false);
                }
            }
        }
    }

    public void Resume()
    {
        resumeBtn.interactable = false;
        menuBtn.interactable = false;
        exitBtn.interactable = false;

        Time.timeScale = 1f;
        uiFadeOut = true;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        uiFadeIn = true;
        GameIsPaused = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        sceneLoadManager.LoadScene("MenuScene");
    }

    public void Exit()
    {
        Debug.Log("Quit Successful");
        Application.Quit();
    }
}
