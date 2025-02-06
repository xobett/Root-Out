using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float actualHealth;
   

    public void TakeDamageAI(float damage) // M�todo para recibir da�o
    {
        Debug.Log("El personaje " + name + " recibio da�o");
        actualHealth -= damage; // Restar el da�o a la vida actual


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
