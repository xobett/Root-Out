using UnityEngine;
using UnityEngine.UI;
using Weapons;
using System;
using UnityEngine.UIElements;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private float damageToPlayer; // Da�o establecido en el inspector
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject playerHitVfx;

    public void SetDamage(float damageAmount)
    {
        damageToPlayer = damageAmount;
    }
    public void Initialize(WeaponsBase weapon) 
    {
        damageToPlayer = weapon.damage; // Asigna el da�o desde WeaponsBase
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer); // Aplica el da�o al jugador

            Instantiate(playerHitVfx, transform.position, hitVfx.transform.rotation);

            Destroy(gameObject);
        }
        if (other.gameObject.TryGetComponent<Sunflower>(out var sunFlower))
        {
            sunFlower.DamageSunFlower(damageToPlayer); // Aplica el da?o al hoja de calabaza
        }

        Instantiate(hitVfx, transform.position, hitVfx.transform.rotation);

        Destroy(gameObject);
    }
}
