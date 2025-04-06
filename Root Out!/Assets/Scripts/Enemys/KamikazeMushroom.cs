using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class KamikazeMushroom : WeaponsBase
{
    [Header("Kamikaze Mushroom Settings")]
    [SerializeField] private float detectionRangeExplosion;
    [SerializeField] private GameObject explosionEffect;

    private Transform sunFlowerGameObject; // Referencia al girasol
    private Sunflower sunFlowerScript; // Referencia al script del girasol
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    private NavMeshAgent agent;

    [SerializeField] private Animator kamikazeAnimatorCtrlr;
    private Transform playerPos;
    [SerializeField] private float playerDetectionRange;

    [SerializeField] private GameObject deathVfx;

    [Header("KAMIKAZE AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionClip;

    protected override void Start()
    {
        sunFlowerScript = FindAnyObjectByType<Sunflower>();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        //Para que no aparezca error comente esta linea... como quiera en el metodo del update esta buscando constantemente un girasol activo, con eso agarra uno.
        //sunFlowerGameObject = GameObject.FindGameObjectWithTag("Sunflower").transform;
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        audioSource.clip = explosionClip;
    }

    protected override void Update()
    {
        Explosion();

        SearchPlayer();

        KamikazeBehaviour();

        GetActiveSunflower();
    }

    private void KamikazeBehaviour()
    {
        if (!agent.isActiveAndEnabled) return;

        if (sunFlowerGameObject != null && GameManager.instance.eventTimerIsActive)
        {
            agent.speed = 3;
            agent.destination = sunFlowerGameObject.transform.position; // Asigna la posición del girasol como destino 

            kamikazeAnimatorCtrlr.SetBool("isRunning", true);
            kamikazeAnimatorCtrlr.SetBool("isWalking", false);
        }
        else if (PlayerDetection())
        {
            kamikazeAnimatorCtrlr.SetBool("isWalking", true);
            kamikazeAnimatorCtrlr.SetBool("isRunning", false);

            agent.speed = 2;
            agent.destination = playerPos.position;
        }
        else
        {
            kamikazeAnimatorCtrlr.SetBool("isWalking", false);
        }
    }

    private void SearchPlayer()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private bool PlayerDetection()
    {
        LayerMask layerMask = LayerMask.GetMask("Player");
        return Physics.CheckSphere(transform.position, playerDetectionRange, layerMask);
    }

    private void GetActiveSunflower()
    {
        if (GameManager.instance.GetActiveSunflower() != null)
        {
            GameObject activeSunflower = GameManager.instance.GetActiveSunflower().gameObject;
            sunFlowerScript = activeSunflower.GetComponent<Sunflower>();
            sunFlowerGameObject = activeSunflower.transform;
        }
    }
    void Explosion()
    {
        if (SunflowerNear())
        {
            SmmokeEffect();

            sunFlowerScript.DamageSunFlower(damage); // Llama al método DamageSunFlower del script Sunflower
            agent.enabled = false;

            Instantiate(deathVfx, transform.position, Quaternion.identity);
            audioSource.Play();
            Destroy(gameObject);
        }
        else if (PlayerNear())
        {
            SmmokeEffect();

            playerPos.gameObject.GetComponent<PlayerHealth>().TakeDamagePlayer(damage);
            agent.enabled = false;

            Instantiate(deathVfx, transform.position, Quaternion.identity);
            audioSource.Play();
            Destroy(gameObject);
        }
    }
    private void SmmokeEffect()
    {
        Quaternion smokeRotation = Quaternion.Euler(-90, 0, 0);
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.6f;

        Instantiate(explosionEffect, spawnPos, smokeRotation);
    }
    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }

    bool PlayerNear()
    {
        LayerMask layerMask = LayerMask.GetMask("Player");
        return Physics.CheckSphere(transform.position, detectionRangeExplosion, layerMask);
    }

    bool SunflowerNear()
    {
        LayerMask layerMask = LayerMask.GetMask("Sunflower");
        return Physics.CheckSphere(transform.position, detectionRangeExplosion, layerMask);
    }
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRangeExplosion);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }

}
