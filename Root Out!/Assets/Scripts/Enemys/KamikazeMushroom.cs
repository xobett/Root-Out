using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class KamikazeMushroom : WeaponsBase
{
    [SerializeField] private float detectionRange;
    [SerializeField] Sunflower sunFlowerScript;
    [SerializeField] Transform _sunFlower;
    [SerializeField] WeaponHandler weaponHandler; // Referencia al WeaponHandler
    NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        StartCoroutine(Attack());
        if (_sunFlower != null)
        {
            agent.SetDestination(_sunFlower.position); 
        }
        GetActiveSunflower();
    }

    private void GetActiveSunflower()
    {
        if (GameManager.instance.activeSunflower != null)
        {
            GameObject activeSunflower = GameManager.instance.activeSunflower.gameObject;
            sunFlowerScript = activeSunflower.GetComponent<Sunflower>();
            _sunFlower = activeSunflower.transform;
        }
    }
    IEnumerator Attack()
    {
        if (Detection())
        {
            Debug.Log("3");
            yield return new WaitForSeconds(1f);
            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            Debug.Log("1");
            yield return new WaitForSeconds(1f);
            Debug.Log("BOOM");
            sunFlowerScript.DamageSunFlower(damage);
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
        return Physics.CheckSphere(transform.position, detectionRange, layerMask);
    }
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
