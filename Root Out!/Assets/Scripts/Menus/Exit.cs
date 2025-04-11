using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private string scene;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

 public void ExitGame()
    {
        SceneManager.LoadSceneAsync(scene);
        Debug.Log("Salir");
    }
}
