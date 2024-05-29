using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    #region GameObjects
    [Header("Game Objects and Images")]
    [SerializeField] private GameObject loadingSceneCanvas;
    [SerializeField] private Image background;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject textBoxFront;
    [SerializeField] private GameObject loadingText;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image progressBarFill;
    #endregion

    #region Hint Texts
    [Header("Hint Texts")]
    [SerializeField] private string[] hintTextArray;
    #endregion

    private YieldInstruction fadeInstruction = new YieldInstruction();
    private float fadeTime = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        // Reset all alphas and setactives
        Color c = background.color;
        c.a = 0;
        background.color = c;

        textBox.SetActive(false);
        loadingText.SetActive(false);
        progressBar.SetActive(false);
        progressBarFill.fillAmount = 0;

        loadingSceneCanvas.SetActive(true);

        RandomHintText();

        await Task.Delay(100);
        StartCoroutine("LoadingCanvas", scene);
    }

    IEnumerator LoadingCanvas(AsyncOperation scene)
    {
        float elapsedTime = 0.0f;
        Color c = background.color;

        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeTime);
            background.color = c;
        }

        yield return new WaitForSeconds(0.5f);

        textBox.SetActive(true);
        loadingText.SetActive(true);
        progressBar.SetActive(true);

        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1);

        do
        {
            yield return new WaitForSeconds(0.1f);
            progressBarFill.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        yield return new WaitForSeconds(1);

        scene.allowSceneActivation = true;

        yield return new WaitForSeconds(0.2f);

        loadingSceneCanvas.SetActive(false);
    }

    public void RandomHintText()
    {
        int randText = Random.Range(0, hintTextArray.Length);

        textBox.GetComponent<TMP_Text>().text = "Tips:<br>" + hintTextArray[randText];
        textBoxFront.GetComponent<TMP_Text>().text = "Tips:<br>" + hintTextArray[randText];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            LoadScene("MenuScene");
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            LoadScene("GameLevel2");
        }
    }
}
