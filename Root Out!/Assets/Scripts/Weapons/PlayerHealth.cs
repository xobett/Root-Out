using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    [SerializeField, Range(0, 100)] public float currentHealth;
    [SerializeField] public Image playerLifeBar;



    public void TakeDamagePlayer(float damage)
    {
        currentHealth -= damage;
        playerLifeBar.fillAmount = currentHealth / maxHealth; // Calcula el fillAmount basado en la vida actual y máxima
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SetPlayerHealth(float health)
    {
        currentHealth = health;
        playerLifeBar.fillAmount = currentHealth / maxHealth;
        Debug.Log("Health was set to its maximum!");
    }

    [ContextMenu("Damage")]
    public void Damage()
    {
        currentHealth -= 20;
        playerLifeBar.fillAmount = currentHealth / maxHealth; // Calcula el fillAmount basado en la vida actual y máxima
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        var playerCharacterCtrlr = gameObject.GetComponent<CharacterController>();
        var lastCheckpointSaved = gameObject.GetComponent<CheckpointUpdater>().lastCheckpoint;

        playerCharacterCtrlr.enabled = false;
        gameObject.transform.position = lastCheckpointSaved;
        playerCharacterCtrlr.enabled = true;
    }


    internal void TryGetComponent<T>()
    {
        // Lanza una excepción que indica que el método no está implementado
        throw new NotImplementedException();
    }

}
