using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Volume globalVolume;
    Vignette vignette;

    [SerializeField] GameObject panelSettings;
    [SerializeField] GameObject panelMain;
    [SerializeField] GameObject panelLogros;

    [SerializeField] Slider mainVolumeSlider; // Referencia al slider de volumen principal
    [SerializeField] AudioSource audioSourceMain; // Referencia al AudioSource principal


    private void Start()
    {
        globalVolume = FindFirstObjectByType<Volume>();
        if (globalVolume.profile.TryGet(out Vignette vignetteComponent))
        {
            vignette = vignetteComponent;
            vignette.intensity.value = 0f; // Inicializar la intensidad de la vi�eta a 0
        }

        panelSettings.SetActive(false); // Activa el men� de configuraci�n
        panelLogros.SetActive(false);

        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor

    }

    public void Game()
    {
        // Asegurarse de que el tiempo de juego est� corriendo
        Time.timeScale = 1;

        AudioManagerSFX.Instance.PlaySFX("Entrada");
        // Iniciar la transici�n de la vi�eta
        StartCoroutine(VignetteTransition());
    }

    private IEnumerator VignetteTransition()
    {
        float duration = 0.5f; // Duraci�n de la transici�n
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            vignette.intensity.value = intensity;
            yield return null;
        }
        vignette.intensity.value = 1f; // Asegurarse de que la intensidad sea 1 al final

        // Cargar la escena "Game" en modo �nico para reiniciarla cada vez
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Quit()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        Application.Quit();
    }
    public void Settings()
    {
        AudioManagerSFX.Instance.PlaySFX("Into");
        panelMain.SetActive(false);
        panelSettings.SetActive(true); // Activa el men� de configuraci�n
    }
    public void Back()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        panelMain.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void Logros()
    {
        AudioManagerSFX.Instance.PlaySFX("Into");
        panelMain.SetActive(false);
        panelLogros.SetActive(true);
    }
    public void BackLogros()
    {
        AudioManagerSFX.Instance.PlaySFX("Out");
        panelMain.SetActive(true);
        panelLogros.SetActive(false);
    }
    public void Volumen()
    {
        mainVolumeSlider.onValueChanged.AddListener(SetMainVolume); // A�ade un listener para el slider de volumen principal
        mainVolumeSlider.value = audioSourceMain.volume; // Inicializa el slider con el valor actual del volumen principal
    }

    // M�todo para actualizar el volumen principal
    public void SetMainVolume(float volume)
    {
        audioSourceMain.volume = volume; // Establece el volumen del AudioSource principal
    }
}

