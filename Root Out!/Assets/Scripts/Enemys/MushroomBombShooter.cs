

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class MushroomBombShooter : WeaponsBase
{
    [Header("Mushroom Bomb Shooter Settings")]
    [SerializeField] private GameObject HUDTargetPoint; // prefab del HUD taget point que es la imagen tiro al blanco en el piso
    [SerializeField] private float heightSecondBullet = 12f; // Mide la altura de la bala que aparece arriba del player
    [SerializeField] private GameObject smokePrefab;


    private Transform player; // Objetivo a disparar
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler

    private Vector3 pointFloor; // Posición de la imagen
    private GameObject point; // Imagen
    private Transform bombMark;

    NavMeshAgent agent;
    [SerializeField] private Animator bombShooterAnimCtrlr;
    private float mushroomSpeed;


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

        // Verificar si bombMark sigue existiendo antes de acceder a su posición
        if (bombMark != null)
        {
            bombMark.position = new Vector3(player.position.x, 0.6f, player.position.z);
        }
        else
        {
            Debug.LogWarning("El objeto 'bombMark' ha sido destruido o no está asignado.");
        }
    }

    private void LateUpdate()
    {
        LookAtTarget(player); // Llama al método LookAtTarget

        mushroomSpeed = agent.velocity.magnitude;

        if (mushroomSpeed > 0)
        {
            bombShooterAnimCtrlr.SetBool("isShooting", false);
            bombShooterAnimCtrlr.SetBool("isWalking", true);
        }
        else
        {

            bombShooterAnimCtrlr.SetBool("isShooting", true);
            bombShooterAnimCtrlr.SetBool("isWalking", false);
        }
    }

    protected override void Shoot()
    {
        if (mushroomSpeed == 0)
        {
            AudioManagerSFX.Instance.PlaySFX("HongoBomba"); // Sonido de disparo

            SmokeShoot();
            Mortar(); // Llama al método Mortar
            base.Shoot(); // Llama al método Shoot de la clase base
            Attack(); // Llama al método Attack 
        }
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

    private void SmokeShoot()
    {
        Quaternion smokeOrientation = Quaternion.Euler(-90, 0, 0);

        GameObject explosion = Instantiate(smokePrefab, aiming.position, smokeOrientation);

        Destroy(explosion, 4);
    }
    IEnumerator TargetPointCoroutine() // Corrutina para instanciar targetShooting
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate); // Espera el tiempo basado en la cadencia de disparo

            // Verificar si bombMark sigue existiendo antes de acceder a su posición
            if (bombMark != null)
            {
                pointFloor = bombMark.position + Vector3.down * 0; // Posición de la imagen
                if (mushroomSpeed == 0)
                {
                    point = Instantiate(HUDTargetPoint, pointFloor, Quaternion.identity); // Instancia de la imagen 
                    Destroy(point, 1.5f); // Destruye la imagen después de 1.5 segundos
                }
            }
            else
            {
                Debug.LogWarning("El objeto 'bombMark' ha sido destruido o no está asignado.");
                yield break; // Salir de la corrutina si bombMark ya no existe
            }

            yield return null;
        }
    }

    void LookAtTarget(Transform target) // Método para mirar al jugador
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
}
