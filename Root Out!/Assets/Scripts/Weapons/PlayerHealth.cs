using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
   // [SerializeField, Range(0,100)] private int maxHealth = 100;
    [SerializeField, Range(0,100)] private int currentHealth;
    [SerializeField] Image lifeBar;


    private void Update()
    {
        LifeBar();
    }
    public void TakeDamagePlayer(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
        }
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
