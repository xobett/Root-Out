using UnityEngine;

public class Exit : MonoBehaviour
{
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

 public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
