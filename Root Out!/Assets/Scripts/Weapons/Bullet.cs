using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private int damage; // Daño de la bala

    public void SetDamage(int damageAmount) // Método para asignar el daño
    {
        damage = damageAmount; // Asignar el daño
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Intentar obtener el componente AIHealth
        {
            aiHealth.TakeDamageAI(damage); // Convertir el daño a entero y aplicarlo
        }

        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}


