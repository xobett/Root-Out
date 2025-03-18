using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    private bool canInstantiateExplosion = false; // Controla si se puede instanciar la explosi�n
    [SerializeField] public GameObject explosionPrefab;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    public void SetExplosionPrefab(GameObject prefab) // M�todo para asignar el prefab de la explosi�n
    {
        explosionPrefab = prefab; // Asignar el prefab
    }

    public void SetCanInstantiateExplosion(bool value) // M�todo para controlar la instanciaci�n de la explosi�n
    {
        canInstantiateExplosion = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar da�o al AIHealth
            Destroy(gameObject); // Destruir la bala
        }

        if (canInstantiateExplosion && explosionPrefab != null) // Si se puede instanciar la explosi�n y el prefab no es nulo
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Instanciar el prefab
        }
        Destroy(gameObject, 1f); // Destruir la bala


    }
}


