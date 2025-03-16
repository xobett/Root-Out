using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Volume globalVolume;
    Vignette vignette;

    private void Start()
    {
        globalVolume = FindFirstObjectByType<Volume>();
        if (globalVolume.profile.TryGet(out Vignette vignetteComponent))
        {
            vignette = vignetteComponent;
            vignette.intensity.value = 0f; // Inicializar la intensidad de la vi�eta a 0
        }
    }

    public void Game()
    {
        // Asegurarse de que el tiempo de juego est� corriendo
        Time.timeScale = 1;

        AudioManager.instance.PlaySFX("Entrada");
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
}

