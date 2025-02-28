
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Weapons;

public class MushroomBombShooter : WeaponsBase
{
    [SerializeField] private Transform player;
    [SerializeField] GameObject targetShooting;
   // [SerializeField] float separationDistance = 1f;

    NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(TargetPointCoroutine()); // Inicia la corrutina para instanciar targetShooting
    }

    protected override void Shoot()
    {
        Mortar(); // Llama al m�todo Mortar
        base.Shoot(); // Llama al m�todo Shoot de la clase base
        Attack(); // Llama al m�todo Attack
    }

    void Attack()
    {
        agent.SetDestination(player.position);
        LookAtTarget(player);
    }

    void Mortar()
    {
        // Posici�n por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * 12f;

        // Instancia del proyectil
        GameObject mortar = Instantiate(bulletPrefab, mortarPosition, Quaternion.identity);

        // Inicializa el da�o del proyectil
        if (mortar.TryGetComponent<EnemyBullet>(out var enemyBullet))
        {
            enemyBullet.Initialize(this); // Pasa la instancia de WeaponsBase
        }

        // A�ade fuerza en la direcci�n aleatoria
        mortar.GetComponent<Rigidbody>().AddForce(Vector3.down * bulletForce, ForceMode.Impulse);
    }

    IEnumerator TargetPointCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // Espera el tiempo basado en la cadencia de disparo

            Vector3 pointFloor = player.position + Vector3.down * 0f; // Posicio de la imagen
            GameObject point = Instantiate(targetShooting, pointFloor,Quaternion.identity); // instanciar la imagen a la posicion

            Destroy(point,1.3f); // Destruye el punto
        }
    }

    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direcci�n hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotaci�n de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotaci�n suave
    }
}
