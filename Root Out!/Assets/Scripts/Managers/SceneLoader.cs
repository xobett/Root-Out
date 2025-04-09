using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Slider progressBar;
    [SerializeField, Range(0f, 4f)] private float progressBarSpeed;

    [SerializeField] private GameObject loadButton;

    private bool activateScene;

    private float targetValue;


    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public void LoadOnClick()
    {
        activateScene = true;
    }

    void Update()
    {
        progressBar.value = Mathf.MoveTowards(progressBar.value, targetValue, progressBarSpeed * Time.deltaTime);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        loadScene.allowSceneActivation = false;

        do
        {
            yield return new WaitForSeconds(1f);

            float loadProgress = Mathf.Clamp01(loadScene.progress / 0.9f);

            targetValue = loadProgress;

            Debug.Log("Loading");
            Debug.Log(loadScene.progress);
        }
        while (loadScene.progress < 0.9f);

        Debug.Log("Finished loading");

        yield return new WaitUntil(() => progressBar.value == 1);

        loadButton.SetActive(true);

        yield return new WaitUntil(() => activateScene);

        loadScene.allowSceneActivation = true;

        yield return null;
    } 
}
