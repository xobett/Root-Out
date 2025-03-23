
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using System;
public class MushroomShooter : WeaponsBase
{
    [Header("MushroomShooter")]
    [SerializeField] private float detectionPlayer;

    private WeaponHandler weaponHandler;
    private Transform player;
    private Transform sunFlower;
    private NavMeshAgent agent;

    protected override void Start()
    {
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sunFlower = GameObject.FindGameObjectWithTag("Sunflower").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void LateUpdate()
    {
        TargetToAtack();
    }

    protected override void Update()
    {
        base.Update();

        GetActiveSunflower();
    }

    protected override void Reload()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon == gameObject)
        {
            base.Reload();
        }
    }
    protected override void Shoot()
    {
        base.Shoot();
    }

    void TargetToAtack()
    {
        if (Detection())
        {
            agent.SetDestination(player.position);
            LookAtTarget(player);
        }

        else
        {
            if (sunFlower != null)
            {
                agent.SetDestination(sunFlower.position);
                LookAtTarget(sunFlower);
            }
        }
    }

    private void GetActiveSunflower()
    {
        if (GameManager.instance.activeSunflower != null)
        {
            GameObject activeSunflower = GameManager.instance.activeSunflower.gameObject;
            sunFlower = activeSunflower.transform; 
        }
    }
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionPlayer);
    }
    bool Detection()
    {
        LayerMask layerMask = LayerMask.GetMask("Player");
        return Physics.CheckSphere(transform.position, detectionPlayer, layerMask);
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
    
}
