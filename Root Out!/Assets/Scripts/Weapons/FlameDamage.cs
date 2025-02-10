using UnityEngine;

public class FlameDamage : MonoBehaviour, IBullet
{
    private float damage;
    private float continuousDamage;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;

    }
    private void OnCollisionEnter(Collision collision)
    {
        continuousDamage = damage * -1;
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth))
        {
            aiHealth.TakeDamage(damage); 
        }
        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}
