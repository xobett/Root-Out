using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class KamikazeMushroom : WeaponsBase
{
    [Header("Kamikaze Mushroom Settings")]
    [SerializeField] private float detectionRangeExplosion;
    [SerializeField] private float playerDetectionRange;

    private Transform player;

    private Transform sunFlowerGameObject; // Referencia al girasol
    private Sunflower sunFlowerScript; // Referencia al script del girasol
    private WeaponHandler weaponHandler; // Referencia al WeaponHandler
    private NavMeshAgent agent;

    [SerializeField] private Animator kamikazeAnimCtrlr;

    protected override void Start()
    {
        sunFlowerScript = FindAnyObjectByType<Sunflower>();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        //Para que no aparezca error comente esta linea... como quiera en el metodo del update esta buscando constantemente un girasol activo, con eso agarra uno.
        //sunFlowerGameObject = GameObject.FindGameObjectWithTag("Sunflower").transform;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        //StartCoroutine(Explosion());

        if (sunFlowerGameObject != null && GameManager.instance.eventTimerIsActive)
        {
            agent.speed = 3;
            agent.destination = sunFlowerGameObject.transform.position;
            Debug.Log("Follows sunflower");

            kamikazeAnimCtrlr.SetBool("isRunning", true);
            kamikazeAnimCtrlr.SetBool("isWalking", false);

        }
        else if (PlayerDetection())
        {
            kamikazeAnimCtrlr.SetBool("isWalking", true);
            kamikazeAnimCtrlr.SetBool("isRunning", false);

            agent.speed = 2;
            agent.destination = player.transform.position;
        }
        else
        {
            kamikazeAnimCtrlr.SetBool("isWalking", false);
        }

        GetActiveSunflower();
    }

    private bool PlayerDetection()
    {
        LayerMask layerMask = LayerMask.GetMask("Player");
        return Physics.CheckSphere(transform.position, playerDetectionRange, layerMask);
    }

    private void GetActiveSunflower()
    {
        if (GameManager.instance.activeSunflower != null)
        {
            GameObject activeSunflower = GameManager.instance.activeSunflower.gameObject;
            sunFlowerScript = activeSunflower.GetComponent<Sunflower>();
            sunFlowerGameObject = activeSunflower.transform;
        }
    }
    void Explosion()
    {
        if (Detection())
        {
            sunFlowerScript.DamageSunFlower(damage); // Llama al método DamageSunFlower del script Sunflower
            Destroy(gameObject);
            Debug.Log($"Daño al girasol : " + sunFlowerScript.currentHealth);
        }
    }
    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }
    bool Detection()
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
