using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 100;
    [SerializeField, Range(0, 100)] public float currentHealth;
    [SerializeField] private Image playerLifeBar;

    public void TakeDamagePlayer(float damage)
    {
        currentHealth -= damage;
        //playerLifeBar.fillAmount = currentHealth / maxHealth; // Calcula el fillAmount basado en la vida actual y máxima
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("El jugador ha muerto.");
    }
    internal void TryGetComponent<T>()
    {
        // Lanza una excepción que indica que el método no está implementado
        throw new NotImplementedException();
    }

}
