using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar daño al AIHealth
            Destroy(gameObject); // Destruir la bala después de la colisión
        }

    }
}


