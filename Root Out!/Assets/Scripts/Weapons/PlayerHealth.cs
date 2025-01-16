using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
   // [SerializeField, Range(0,100)] private int maxHealth = 100;
    [SerializeField, Range(0,100)] private int currentHealth;
    
   
    public void TakeDamagePlayer(int damage)
    {
        currentHealth -= damage;
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

}
