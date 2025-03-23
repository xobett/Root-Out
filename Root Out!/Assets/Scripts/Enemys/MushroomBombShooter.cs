

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class MushroomBombShooter : WeaponsBase
{
    [Header("Mushroom Bomb Shooter Settings")]
    [SerializeField] private GameObject HUDTargetPoint; // prefab del HUD taget point que es la imagen tiro al blanco en el piso
    [SerializeField] private float targetPointDistance; // Distancia de la imagen "tiro al blanco"
    [SerializeField] private float heightSecondBullet = 12f; // Mide la altura de la bala que aparece arriba del player

    private Transform player; // Objetivo a disparar
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler

    NavMeshAgent agent;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Busca el jugador por la etiqueta
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(TargetPointCoroutine()); // Inicia la corrutina para instanciar targetShooting
    }

    private void LateUpdate()
    {
        LookAtTarget(player); // Llama al método LookAtTarget
    }

    protected override void Shoot()
    {
        Mortar(); // Llama al método Mortar
        base.Shoot(); // Llama al método Shoot de la clase base
        Attack(); // Llama al método Attack
    }

    protected override void Reload() 
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject) // Este metodo evita que la imagen de recarga se active
        {
            base.Reload();
        }
    } 
    void Attack()
    {
        agent.SetDestination(player.position);
    }

    void Mortar()
    {
        // Posición por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * heightSecondBullet;

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

    IEnumerator TargetPointCoroutine() // Corrutina para instanciar targetShooting
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // Espera el tiempo basado en la cadencia de disparo

            Vector3 pointFloor = player.position + Vector3.down * targetPointDistance; // Posicio de la imagen
            GameObject point = Instantiate(HUDTargetPoint, pointFloor, Quaternion.identity); // instanciar la imagen a la posicion

            Destroy(point, 1.3f); // Destruye el punto
        }
    }

    void LookAtTarget(Transform target) // Método para mirar al jugador
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
   
}
