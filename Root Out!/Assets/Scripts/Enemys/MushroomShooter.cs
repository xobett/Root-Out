
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using System;
using TMPro.SpriteAssetUtilities;
public class MushroomShooter : WeaponsBase
{
    [Header("MushroomShooter")]
    [SerializeField] private float detectionPlayer;

    private WeaponHandler weaponHandler;
    private Transform player;
    private Transform sunFlower;
    private NavMeshAgent agent;

    [SerializeField] private Animator mushroomShooterAnimCtrlr;
    private float shooterSpeed;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;

    protected override void Start()
    {
        weaponHandler = FindFirstObjectByType<WeaponHandler>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = gameObject.GetComponent<NavMeshAgent>();

        audioSource.clip = shootClip;
    }
    private void LateUpdate()
    {
        TargetToAtack();
    }

    protected override void Update()
    {
        base.Update();

        GetActiveSunflower();

        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (shooterSpeed == 0 && Detection())
        {
            base.Shoot();
            audioSource.Play();
        }
    }

    void TargetToAtack()
    {
        if (Detection())
        {
            agent.destination = player.transform.position;
            LookAtTarget(player);
        }
        else
        {
            if (sunFlower != null)
            {
                agent.destination = sunFlower.transform.position;
                LookAtTarget(sunFlower);
            }
        }

       // Debug.Log(agent.velocity.magnitude);

        shooterSpeed = agent.velocity.magnitude;

        if (shooterSpeed > 0)
        {
            mushroomShooterAnimCtrlr.SetBool("isShooting", false);
            mushroomShooterAnimCtrlr.SetBool("isWalking", true);
        }
        else
        {
            mushroomShooterAnimCtrlr.SetBool("isShooting", true);
            mushroomShooterAnimCtrlr.SetBool("isWalking", false);
        }
    }

    private void GetActiveSunflower()
    {
        if (GameManager.instance.GetActiveSunflower() != null)
        {
            GameObject activeSunflower = GameManager.instance.GetActiveSunflower().gameObject;
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
