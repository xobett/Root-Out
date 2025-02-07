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
        base.Shoot(); // Llama al m�todo Shoot de la clase base
        Attack(); // Llama al m�todo Attack
        Mortar(); // Llama al m�todo Mortar
    }

    void Attack()
    {
        agent.SetDestination(player.position);
        LookAtTarget(player);
    }

    void Mortar()
    {
        // Posici�n por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * 10f; // 10 unidades por encima del jugador

        // Direcci�n aleatoria
        Vector3 randomDirection = new Vector3(0, Random.Range(-1,1),0).normalized;

        // Instancia del proyectil
        GameObject mortar = Instantiate(bulletPrefab, mortarPosition, Quaternion.identity);

        // A�ade fuerza en la direcci�n aleatoria
        mortar.GetComponent<Rigidbody>().AddForce(Vector3.down * bulletForce, ForceMode.Impulse);
    }

    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direcci�n hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotaci�n de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotaci�n suave
    }
}
