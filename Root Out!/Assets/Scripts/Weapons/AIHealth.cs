using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float actualHealth;
    [SerializeField, Range(0, 100)] private float maxHealth;
    [SerializeField] private Image hUDLifeBar;


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
        Destroy(gameObject);
    }
}
