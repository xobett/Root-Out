using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private int damage;

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth))
        {
            aiHealth.TakeDamage(damage); // Convertir el daño a entero y aplicarlo
        }

        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}


