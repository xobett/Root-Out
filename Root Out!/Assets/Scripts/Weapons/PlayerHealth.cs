using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("PLAYER HEALTH SETTINGS")]
    public float maxHealth = 100;
    [SerializeField, Range(0, 100)] public float currentHealth;

    [SerializeField] private Image lifebarFill;

    [Header("PLAYER DEATH ANIMATION SETTINGS")]
    [SerializeField] private Animator playerAnimCtrlr;
    [SerializeField] private Animator deathPanelAnimCtrlr;

    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

    [SerializeField] private GameObject hud;

    private bool playerIsDead;

    public void TakeDamagePlayer(float damage)
    {
        currentHealth -= damage;

        lifebarFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0 && !playerIsDead)
        {
            StartCoroutine(Die());
        }
    }

    public void SetPlayerHealth(float health)
    {
        currentHealth = health;

        lifebarFill.fillAmount = currentHealth / maxHealth;
        Debug.Log("Health was set to its maximum!");
    }

    [ContextMenu("Damage")]
    public void Damage()
    {
        currentHealth -= 20;

        lifebarFill.fillAmount = currentHealth / maxHealth;
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            AudioManagerSFX.Instance.PlaySFX("Muerte");
            StartCoroutine(Die());

        }
    }

    private IEnumerator Die()
    {
        playerIsDead = true;

        hud.SetActive(false);

        playerAnimCtrlr.SetLayerWeight(1, 0);

        playerAnimCtrlr.SetTrigger("Death");

        var playerMovement = gameObject.GetComponent<PlayerMovement>();
        var playerCropHandler = gameObject.GetComponent<CropHandler>();
        var playerLeafJump = gameObject.GetComponent<LeafJump>();
        var cameraFollow = Camera.main.GetComponent<CameraFollow>();

        //Desactiva el input del player.
        playerMovement.enabled = false;
        playerCropHandler.enabled = false;
        playerLeafJump.enabled = false;
        cameraFollow.enabled = false;

        //yield return new WaitForSecondsRealtime(2f);
        
        volume.profile.TryGet(out colorAdjustments);

        float duration = 5f;
        float elapsed = 0f;
        float startSaturation = 50f;
        float endSaturation = -100f;

        while (elapsed < duration)
        {
            Time.timeScale = Mathf.Lerp(1f, 0f, elapsed/duration);
            colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, endSaturation, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }


        deathPanelAnimCtrlr.SetTrigger("Intro");
        Cursor.lockState = CursorLockMode.None;
    }

    internal void TryGetComponent<T>()
    {
        // Lanza una excepción que indica que el método no está implementado
        throw new NotImplementedException();
    }

}
