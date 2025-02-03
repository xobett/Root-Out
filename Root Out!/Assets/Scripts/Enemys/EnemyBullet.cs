using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private int damageToPlayer;
    private int damageSunFlower;

    public void SetDamage(int damageAmount)
    {
        damageToPlayer = damageAmount;
        damageSunFlower = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer);
        }
        if (collision.collider.TryGetComponent<Sunflower>(out var sunflower))
        {
            sunflower.DamageSunFlower(damageSunFlower);
        }
        Destroy(gameObject);
    }

}
