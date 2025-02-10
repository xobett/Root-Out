using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //[SerializeField, Range(0,100)] private float maxHealth = 100;
    [SerializeField, Range(0,100)] private float currentHealth;
    
   
    public void TakeDamagePlayer(float damage)
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
