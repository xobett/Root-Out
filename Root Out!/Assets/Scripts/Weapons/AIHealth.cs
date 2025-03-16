using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float actualHealth;
    [SerializeField, Range(0, 100)] private float maxHealth;
    [SerializeField] private Image hUDLifeBar;
    public static int enemiesDefeated = 0; // Variable estática para contar los enemigos derrotados

    public void TakeDamage(float damage)
    {
        Debug.Log("El personaje " + name + " recibio daño");
        actualHealth -= damage;
        hUDLifeBar.fillAmount = actualHealth / maxHealth;

        if (actualHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Mataste a " + name);
        enemiesDefeated++; // Incrementar el contador de enemigos derrotados
        Destroy(gameObject);
    }
}
