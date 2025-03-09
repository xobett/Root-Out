
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class MushroomShooter : WeaponsBase
{
    private NavMeshAgent agent;
    [Header("MushroomShooter")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform sunFlower;
    [SerializeField] private float rangeGizmo;
    [SerializeField] LayerMask layer;


    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void LateUpdate()
    {
        TargetToAtack();

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
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeGizmo);
    }
    bool Detection()
    {
        return Physics.CheckSphere(transform.position, rangeGizmo, layer);
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Dirección hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotación de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotación suave
    }
}
