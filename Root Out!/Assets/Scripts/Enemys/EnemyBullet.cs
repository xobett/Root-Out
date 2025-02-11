using UnityEngine;
using UnityEngine.UI;
using Weapons;

public class EnemyBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float damageToPlayer; // Daño establecido en el inspector

    public void SetDamage(float damageAmount)
    {
        damageToPlayer = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer); // Aplica el daño al jugador
        }
        Destroy(gameObject);
    }
    public void Initialize(WeaponsBase weapon)
    {
        damageToPlayer = weapon.damage; // Asigna el daño desde WeaponsBase
    }
}
