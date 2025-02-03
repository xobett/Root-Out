using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Random = UnityEngine.Random;

public class MushroomBombShooter : WeaponsBase
{
    [SerializeField] private Transform player;
    NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SpawnBulletsAroundPlayer()); // Inicia la corrutina para disparar balas alrededor del jugador
    }

    protected override void Update()
    {
        Attack();
    }

    protected override void Shoot()
    {
        base.Shoot();
    }

    void Attack()
    {
        agent.SetDestination(player.position);
        LookAtTarget(player);
    }

    private IEnumerator SpawnBulletsAroundPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // Espera el tiempo de la cadencia de disparo

            Vector3 randomOffset = new Vector3(
                Random.Range(-1.5f, 1.5f), // Desplazamiento aleatorio en X
                Random.Range(12f, 12f), // Desplazamiento aleatorio en Y (por encima del jugador)
                Random.Range(-3f, 3f) // Desplazamiento aleatorio en Z
            );

            Vector3 spawnPosition = player.position + randomOffset; // Posición de aparición de la bala
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity); // Instancia la bala
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.down * bulletForce, ForceMode.Impulse); // Aplica una fuerza hacia abajo

            // Verifica si el prefab de la bala tiene el componente IBullet
            IBullet bulletComponent = bullet.GetComponent<IBullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDamage(damage); // Establece el daño de la bala
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have BulletDamage component.");
            }

            Destroy(bullet, lifeTimeBullets); // Destruye la bala después de que expire el tiempo de vida especificado.
        }
    }

    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
}
