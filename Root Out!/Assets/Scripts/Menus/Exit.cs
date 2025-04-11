using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

 public void ExitGame()
    {
        var sceneToLoad = SceneManager.GetSceneByName("Main Menu");
        SceneManager.LoadSceneAsync(sceneToLoad.buildIndex);
        Debug.Log("Salir");
    }
}
