using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private float damageToPlayer;

    public void SetDamage(float damageAmount)
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
