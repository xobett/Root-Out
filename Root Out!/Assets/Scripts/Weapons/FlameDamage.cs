using UnityEngine;

public class FlameDamage : MonoBehaviour, IBullet
{
    private int damage;
    private int continuousDamage;

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;

    }
    private void OnCollisionEnter(Collision collision)
    {
        continuousDamage = damage * -1;
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth))
        {
            aiHealth.TakeDamageAI(damage); 
        }
        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
}
