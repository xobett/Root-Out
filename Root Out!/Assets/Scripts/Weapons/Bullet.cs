using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    [SerializeField] public GameObject explosionPrefab;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }
    public void SetExplosionPrefab(GameObject prefab)
    {
        explosionPrefab = prefab;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar daño al AIHealth
        }
        if (explosionPrefab != null) // Si el prefab de la explosión no es nulo
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instanciar el prefab

            // Destruir la bala al colisionar
            Destroy(gameObject);
        }
    }
    public void Explosion()
    {
       Debug.Log("Explosion");
    }

}


