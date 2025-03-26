using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private float damage;
    public GameObject explosionPrefab;
    public bool canExplode = true;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AIHealth>(out var aiHealth)) // Si el objeto colisionado tiene un componente AIHealth
        {
            aiHealth.TakeDamage(damage); // Aplicar da�o al AIHealth
            if (canExplode)
            {
                StartCoroutine(ExplosionCooldown()); // Iniciar la corrutina de explosi�n
                Destroy(gameObject,2f); // Destruir la bala despu�s de 2 segundos
            }
        }
    }

    public IEnumerator ExplosionCooldown()  // Corrutina para controlar el tiempo de espera entre instanciaciones de explosi�n
    {
        while (canExplode)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            //bool canExplode = false;
            Debug.Log("Exploto");
        }
    }
}


