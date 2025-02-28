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
        }

        // Destruir la bala al colisionar
        Destroy(gameObject);
    }
    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos
        Explosion(); // Llama al método Explosion
    }

}


