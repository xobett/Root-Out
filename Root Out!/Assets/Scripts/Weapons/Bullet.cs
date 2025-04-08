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
            GameObject enemyVfx = Instantiate(hitEnemyVfx, transform.position, hitEnemyVfx.transform.rotation);

            AudioManagerSFX.Instance.PlaySFX("Enemy hit");

            aiHealth.TakeDamage(damage);

            Destroy(enemyVfx, 1);

            Destroy(gameObject);
        }

        if (GameManager.instance.explosionUpgradeActivated)
        {
            GameObject explosiveVfx = Instantiate(explosiveHitVfx, transform.position, explosiveHitVfx.transform.rotation);

            Destroy(explosiveVfx, 1);
        }
        else
        {
            GameObject normalVfx = Instantiate(hitVfx, transform.position, hitVfx.transform.rotation);
            Destroy(normalVfx, 1);
        }

        Destroy(gameObject); // Destruir la bala después de la colisión
    }
}


