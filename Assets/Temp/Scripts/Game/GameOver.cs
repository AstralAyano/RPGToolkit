using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private CanvasGroup gameOverGroup;

    [SerializeField] private TMP_Text backText;
    [SerializeField] private TMP_Text frontText;

    [SerializeField] private Button menuBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private float fadeSpeed = 4;

    private bool uiFadeIn = false;

    void Start()
    {
        //sceneLoadManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneLoadManager>();
        gameOverGroup.alpha = 0.0f;
        
        gameOverGroup.gameObject.SetActive(false);
        menuBtn.interactable = false;
        exitBtn.interactable = false;
    }

    void Update()
    {
        if (uiFadeIn)
        {
            if (gameOverGroup.alpha < 1.0f)
            {
                gameOverGroup.alpha += Time.deltaTime * fadeSpeed;

                if (gameOverGroup.alpha >= 1.0f)
                {
                    uiFadeIn = false;
                    Time.timeScale = 0f;
                    menuBtn.interactable = true;
                    exitBtn.interactable = true;
                }
            }
        }
    }

    public void ActivateScene(bool playerWin)
    {
        gameOverGroup.gameObject.SetActive(true);

        if (playerWin)
        {
            frontText.color = new Color32(153, 255, 129, 255);
            backText.text = "You Win !!!";
            frontText.text = "You Win !!!";
        }
        else
        {
            frontText.color = new Color32(255, 129, 139, 255);
            backText.text = "You Lose ...";
            frontText.text = "You Lose ...";
        }

        uiFadeIn = true;
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
