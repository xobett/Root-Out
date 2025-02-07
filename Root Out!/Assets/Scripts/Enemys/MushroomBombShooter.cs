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
    }

    protected override void Shoot()
    {
        base.Shoot(); // Llama al método Shoot de la clase base
        Attack(); // Llama al método Attack
        Mortar(); // Llama al método Mortar
    }

    void Attack()
    {
        agent.SetDestination(player.position);
        LookAtTarget(player);
    }

    void Mortar()
    {
        // Posición por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * 10f; // 10 unidades por encima del jugador

        // Dirección aleatoria
        Vector3 randomDirection = new Vector3(0, Random.Range(-1,1),0).normalized;

        // Instancia del proyectil
        GameObject mortar = Instantiate(bulletPrefab, mortarPosition, Quaternion.identity);

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
