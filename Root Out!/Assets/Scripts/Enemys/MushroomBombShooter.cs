

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class MushroomBombShooter : WeaponsBase
{
    [Header("Mushroom Bomb Shooter Settings")]
    [SerializeField] private GameObject HUDTargetPoint; // prefab del HUD taget point que es la imagen tiro al blanco en el piso
    [SerializeField] private float heightSecondBullet = 12f; // Mide la altura de la bala que aparece arriba del player


    private Transform player; // Objetivo a disparar
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler

    private Vector3 pointFloor; // Posici�n de la imagen
    private GameObject point; // Imagen
    private Transform bombMark;

    NavMeshAgent agent;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Busca el jugador por la etiqueta
        bombMark = GameObject.Find("BombMark").transform;
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(TargetPointCoroutine()); // Inicia la corrutina para instanciar targetShooting
    }

    protected override void Update()
    {
        base.Update();
        // Mantener la posici�n en Y de bombMark constante
        bombMark.position = new Vector3(player.position.x, 0.6f, player.position.z);
    }

    private void LateUpdate()
    {
        LookAtTarget(player); // Llama al m�todo LookAtTarget
    }

    protected override void Shoot()
    {
        Mortar(); // Llama al m�todo Mortar
        base.Shoot(); // Llama al m�todo Shoot de la clase base
        Attack(); // Llama al m�todo Attack
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
        // Posici�n por encima del jugador
        Vector3 mortarPosition = player.position + Vector3.up * heightSecondBullet;

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

    IEnumerator TargetPointCoroutine() // Corrutina para instanciar targetShooting
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // Espera el tiempo basado en la cadencia de disparo

            pointFloor = bombMark.position + Vector3.down * 0; // Posici�n de la imagen
            point = Instantiate(HUDTargetPoint, pointFloor, Quaternion.identity); // Instancia de la imagen
            Destroy(point, 1.5f); // Destruye la imagen despu�s de 0.5 segundos
        }
    }

    void LookAtTarget(Transform target) // M�todo para mirar al jugador
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direcci�n hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotaci�n de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotaci�n suave
    }
}
