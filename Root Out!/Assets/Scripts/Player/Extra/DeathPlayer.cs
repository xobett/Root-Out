using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class DeathPlayer : MonoBehaviour
{
    private PlayerHealth playerHealth;
    //private Animation animDeath;

    private ColorAdjustments colorAdjustments;
    private Volume volume;

    private void Start()
    {
        volume = FindFirstObjectByType<Volume>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void Update()
    {
       StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        volume.profile.TryGet(out colorAdjustments);
        if (playerHealth.currentHealth <= 0)
        {
            //animDeath.Play("Death");
            float duration = 2f;
            float elapsed = 0f; 
            float startSaturation = 50f; 
            float endSaturation = -100f;

            while (elapsed < duration) 
            {
                elapsed += Time.deltaTime; 
                colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, endSaturation, elapsed / duration);
                yield return null;
            }

            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Main Menu");
            StopCoroutine(Death());
        }
    }
}
