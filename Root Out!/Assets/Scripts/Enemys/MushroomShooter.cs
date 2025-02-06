using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class MushroomShooter : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform sunFlower;
    [SerializeField] private float range;
    [SerializeField] LayerMask layer;
    [SerializeField] WeaponsBase aim;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        TargetToAtack();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    bool Detection()
    { 
        return Physics.CheckSphere(transform.position, range, layer);
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direction to the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Look rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Smooth rotation
    }
}
