using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth))
        {
            aiHealth.TakeDamage(damage); 
        }

        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}


