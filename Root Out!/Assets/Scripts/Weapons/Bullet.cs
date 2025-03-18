using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    private bool canInstantiateExplosion = false; // Controla si se puede instanciar la explosión
    [SerializeField] public GameObject explosionPrefab;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    public void SetExplosionPrefab(GameObject prefab) // Método para asignar el prefab de la explosión
    {
        explosionPrefab = prefab; // Asignar el prefab
    }

    public void SetCanInstantiateExplosion(bool value) // Método para controlar la instanciación de la explosión
    {
        canInstantiateExplosion = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar daño al AIHealth
            Destroy(gameObject); // Destruir la bala
        }

        if (canInstantiateExplosion && explosionPrefab != null) // Si se puede instanciar la explosión y el prefab no es nulo
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instanciar el prefab
        }
        Destroy(gameObject, 1f); // Destruir la bala


    }
}


