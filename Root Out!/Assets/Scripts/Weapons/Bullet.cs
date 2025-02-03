using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private int damage; // Da�o de la bala

    public void SetDamage(int damageAmount) // M�todo para asignar el da�o
    {
        damage = damageAmount; // Asignar el da�o
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Intentar obtener el componente AIHealth
        {
            aiHealth.TakeDamageAI(damage); // Convertir el da�o a entero y aplicarlo
        }

        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}


