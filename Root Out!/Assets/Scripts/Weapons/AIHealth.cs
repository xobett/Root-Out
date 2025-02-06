using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float actualHealth;
   

    public void TakeDamageAI(float damage) // Método para recibir daño
    {
        Debug.Log("El personaje " + name + " recibio daño");
        actualHealth -= damage; // Restar el daño a la vida actual


        if (actualHealth < 0)
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
