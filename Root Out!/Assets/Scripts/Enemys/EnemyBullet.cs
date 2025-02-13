using UnityEngine;
using UnityEngine.UI;
using Weapons;
using System;

public class EnemyBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float damageToPlayer; // Daño establecido en el inspector

    public void SetDamage(float damageAmount)
    {
        damageToPlayer = damageAmount;
    }
    public void Initialize(WeaponsBase weapon) 
    {
        damageToPlayer = weapon.damage; // Asigna el daño desde WeaponsBase
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer); // Aplica el daño al jugador
        }
        if(collision.collider.TryGetComponent<Sunflower>(out var sunFlower))
        {
            sunFlower.DamageSunFlower(damageToPlayer); // Aplica el da?o al hoja de calabaza
        }
        Destroy(gameObject);
    }
}
