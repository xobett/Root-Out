using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject explosiveHitVfx;

    [SerializeField] private GameObject hitEnemyVfx;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            Instantiate(hitEnemyVfx, transform.position, hitEnemyVfx.transform.rotation);

            AudioManager.instance.PlaySFX("Enemy hit");

            aiHealth.TakeDamage(damage);

            Destroy(gameObject);
        }

        if (GameManager.instance.explosionUpgradeActivated)
        {
            Instantiate(explosiveHitVfx, transform.position, explosiveHitVfx.transform.rotation);
        }
        else
        {
            Instantiate(hitVfx, transform.position, hitVfx.transform.rotation);
        }

        Destroy(gameObject); // Destruir la bala después de la colisión
    }
}


