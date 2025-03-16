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
            vignette.intensity.value = 0f; // Inicializar la intensidad de la viñeta a 0
        }
    }

    public void Game()
    {
        // Asegurarse de que el tiempo de juego esté corriendo
        Time.timeScale = 1;

        AudioManager.instance.PlaySFX("Entrada");
        // Iniciar la transición de la viñeta
        StartCoroutine(VignetteTransition());
    }

    private IEnumerator VignetteTransition()
    {
        float duration = 0.5f; // Duración de la transición
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float intensity = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            vignette.intensity.value = intensity;
            yield return null;
        }
        vignette.intensity.value = 1f; // Asegurarse de que la intensidad sea 1 al final

        // Cargar la escena "Game" en modo único para reiniciarla cada vez
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}

