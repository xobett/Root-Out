using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int currentHealth;
    [SerializeField] Image lifeBar;

    public void TakeDamagePlayer(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        // Actualiza la barra de vida
        LifeBar();
    }

    void Die()
    {
        Debug.Log("El jugador ha muerto.");
    }

    void LifeBar()
    {
        lifeBar.fillAmount = currentHealth / 100f;
    }
}
