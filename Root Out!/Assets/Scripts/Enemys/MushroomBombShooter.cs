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
    }

    void Attack()
    {
        agent.SetDestination(player.position); 
        LookAtTarget(player); 
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
}
