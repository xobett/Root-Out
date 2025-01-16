using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private int damageToPlayer = 10;

    public void SetDamage(int damageAmount)
    {
        damageToPlayer = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer);
        }
        Destroy(gameObject);
    }

}
