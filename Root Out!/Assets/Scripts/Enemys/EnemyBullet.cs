using UnityEngine;
using UnityEngine.UI;
using Weapons;
using System;
using UnityEngine.UIElements;

public class EnemyBullet : MonoBehaviour, IBullet
{
    private float damageToPlayer; // Daño establecido en el inspector
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject playerHitVfx;

    public void SetDamage(float damageAmount)
    {
        damageToPlayer = damageAmount;
    }
    public void Initialize(WeaponsBase weapon) 
    {
        damageToPlayer = weapon.damage; // Asigna el daño desde WeaponsBase
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamagePlayer(damageToPlayer); // Aplica el daño al jugador

            GameObject spawnVfx = Instantiate(playerHitVfx, transform.position, hitVfx.transform.rotation);

            AudioManager.instance.PlaySFX("Player hit");

            Destroy(spawnVfx, 1);

            Destroy(gameObject);
        }
        if (other.gameObject.TryGetComponent<Sunflower>(out var sunFlower))
        {
            sunFlower.DamageSunFlower(damageToPlayer); // Aplica el da?o al hoja de calabaza
        }

        GameObject vfx = Instantiate(hitVfx, transform.position, hitVfx.transform.rotation);

        Destroy(vfx, 1);

        Destroy(gameObject);
    }
}
