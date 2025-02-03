using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class KamikazeMushroom : WeaponsBase
{
    [SerializeField] private float detectionRange;
    [SerializeField] Sunflower sunFlower;
    [SerializeField] Transform _sunFlower;
    NavMeshAgent agent;

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        StartCoroutine(Attack());
        agent.SetDestination(_sunFlower.position);
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
            sunFlower.DamageSunFlower(damage);
            Destroy(gameObject);


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
