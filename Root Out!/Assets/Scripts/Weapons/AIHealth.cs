using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int actualHealth;
   

    public void TakeDamage(int damage)
    {
        Debug.Log("El personaje " + name + " recibio da�o");
        actualHealth -= damage;
        

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
