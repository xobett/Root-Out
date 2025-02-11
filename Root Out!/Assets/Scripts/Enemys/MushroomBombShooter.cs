using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Weapons;
using Random = UnityEngine.Random;

public class MushroomBombShooter : WeaponsBase
{
    [SerializeField] private Transform player;
    //[SerializeField] private GameObject bomb;

    NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Shoot()
    {
        Mortar(); // Llama al método Mortar
        base.Shoot(); // Llama al método Shoot de la clase base
        Attack(); // Llama al método Attack
    }
    

    void Attack()
    {
        agent.SetDestination(player.position);
        LookAtTarget(player);
    }

    void Mortar()
    {

        // Posición por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * 8f;

        // Dirección aleatoria
        //Vector3 randomDirection = new Vector3(0, Random.Range(-1, 1), 0).normalized;

        // Instancia del proyectil
        GameObject mortar = Instantiate(bulletPrefab, mortarPosition, Quaternion.identity);

        // Inicializa el daño del proyectil
        if (mortar.TryGetComponent<EnemyBullet>(out var enemyBullet)) 
        {
            enemyBullet.Initialize(this); // Pasa la instancia de WeaponsBase
        }


        // Añade fuerza en la dirección aleatoria
        mortar.GetComponent<Rigidbody>().AddForce(Vector3.down * bulletForce, ForceMode.Impulse);
    }

    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
}
