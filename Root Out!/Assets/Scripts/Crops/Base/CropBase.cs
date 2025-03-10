
using UnityEngine;

public abstract class CropBase : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField] protected CropType cropType;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField, Range(0f, 1f)] protected float cropWalkSpeed;
    [SerializeField] protected float cropRunSpeed;

    [SerializeField, Range(0.5f, 10f)] protected float lookAtPlayerSpeed;

    [Header("FOLLOW SETTINGS")]
    [SerializeField, Range(0,5)] protected float stoppingDistance;
    [SerializeField] protected float backDistance;
    [SerializeField] protected float sideDistance;
    
    private Transform playerPos;

    private Transform followTarget;

    private Vector3 velocityRef = Vector3.zero;

    [Header("COMBAT SETTINGS")]
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float damage;

    [Header("ENEMY DETECTION")]
    [SerializeField] private float sphereDetectionRadius = 12f;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float maxHitDistance;
    [SerializeField] private float currentHitDistance;

    private RaycastHit hit;


    private void Start()
    {
        GetReferemces();
    }


    private void Update()
    {
        FollowPlayer();
    }


    protected virtual void FollowPlayer()
    {
        //Rota constantemente hacia el jugador mientras lo sigue.
        LookAtPlayer();

        if (EnemyDetection())
        {
            currentHitDistance = hit.distance;
            Debug.Log($"{hit.collider.name}");
        }
        else
        {
            currentHitDistance = maxHitDistance;
        }

        Vector3 desiredFollowingPos = playerPos.position + playerPos.forward * -backDistance + transform.right * sideDistance;
        desiredFollowingPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        if (distance > stoppingDistance)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredFollowingPos, ref velocityRef, 1f / cropWalkSpeed);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * currentHitDistance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * currentHitDistance, sphereDetectionRadius);
    }

    private bool EnemyDetection() => Physics.SphereCast(transform.position, sphereDetectionRadius, transform.forward, out hit, maxHitDistance, whatIsEnemy);

    private void LookAtPlayer()
    {
        //Gets direction to face
        Vector3 direction = playerPos.position - transform.position;

        //Creates a quaternion that will look at the direction set.
        Quaternion lookAtPlayer = Quaternion.LookRotation(direction, Vector3.up);

        Quaternion newQuat = Quaternion.Euler(0, lookAtPlayer.eulerAngles.y, 0);

        float newAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        //Debug.Log(newAngle);

        //Sets the new rotation.
        transform.rotation = newQuat;

    }

    protected abstract void Ability();

    protected void BeginCooldownTime(float cooldownTime)
    {

    }

    #region Reference Methods
    private void GetReferemces()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion
}
