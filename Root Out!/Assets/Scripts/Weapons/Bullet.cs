using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject explosiveHitVfx;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar da�o al AIHealth
        }

        if (GameManager.instance.explosionUpgradeActivated)
        {
            Instantiate(explosiveHitVfx, transform.position, explosiveHitVfx.transform.rotation);
        }
        else
        {
            Instantiate(hitVfx, transform.position, hitVfx.transform.rotation);
        }

        Destroy(gameObject); // Destruir la bala despu�s de la colisi�n
    }
}


