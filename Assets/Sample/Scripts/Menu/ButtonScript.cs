using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject MenuUI;

    void Start()
    {
        Time.timeScale = 1;
        MenuUI.SetActive(false);
    }

    public void PauseButton()
    {
        MenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        MenuUI.SetActive(false);
    }

    public void MenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
