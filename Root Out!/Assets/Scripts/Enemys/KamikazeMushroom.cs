using System.Collections;
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


    protected override void Start()
    {
        sunFlowerScript = FindAnyObjectByType<Sunflower>();
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        //Para que no aparezca error comente esta linea... como quiera en el metodo del update esta buscando constantemente un girasol activo, con eso agarra uno.
        //sunFlowerGameObject = GameObject.FindGameObjectWithTag("Sunflower").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        StartCoroutine(Explosion());
        if (sunFlowerGameObject != null)
        {
            agent.SetDestination(sunFlowerGameObject.position); // Asigna la posición del girasol como destino
        }
        GetActiveSunflower();
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
    IEnumerator Explosion()
    {
        if (Detection())
        {
            Debug.Log("3");
            yield return new WaitForSeconds(1f);
            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            Debug.Log("1");
            yield return new WaitForSeconds(1f);
            SmmokeEffect();
            Debug.Log("BOOM");
            sunFlowerScript.DamageSunFlower(damage); // Llama al método DamageSunFlower del script Sunflower
            Destroy(gameObject);
            Debug.Log($"Daño al girasol : " + sunFlowerScript.currentHealth);


        }
    }
    private void SmmokeEffect()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
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
    }

}
