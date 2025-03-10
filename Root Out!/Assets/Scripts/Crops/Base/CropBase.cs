
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class CropBase : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField] protected CropType cropType;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField, Range(0f, 1f)] protected float cropWalkSpeed;
    [SerializeField, Range(0f, 1f)] protected float cropRunSpeed;

    [SerializeField, Range(0.5f, 10f)] protected float lookAtPlayerSpeed;

    [Header("FOLLOW SETTINGS")]
    [SerializeField, Range(0, 5)] protected float stoppingDistance;
    [SerializeField] protected float backDistance;
    [SerializeField] protected float sideDistance;

    private Transform playerPos;

    private Vector3 velocityRef = Vector3.zero;

    [Header("COMBAT SETTINGS")]
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float damage;

    [Header("ENEMY DETECTION")]
    [SerializeField] private float sphereDetectionRadius = 12f;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private bool enemyDetected;
    [SerializeField] private Transform enemyPos;

    [SerializeField] private float maxHitDistance;

    private RaycastHit hit;


    private void Start()
    {
        GetReferences();
    }


    private void Update()
    {
        BehaviourCheck();
    }

    protected void BehaviourCheck()
    {
        if (EnemyDetection() && !enemyDetected)
        {
            enemyDetected = true;
            enemyPos = hit.collider.transform;
        }

        if (!enemyDetected)
        {
            HeadToPlayer();
        }
        else
        {
            CropAttack();
        }
    }

    protected void HeadToEnemy()
    {
        Vector3 desiredFollowingPos = enemyPos.position;
        desiredFollowingPos.y = transform.position.y;

        SetDestination(desiredFollowingPos, cropRunSpeed);
    }

    private void HeadToPlayer()
    {
        LookAtPlayer();

        Vector3 desiredFollowingPos = playerPos.position + playerPos.forward * -backDistance + transform.right * sideDistance;
        desiredFollowingPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        if (distance > stoppingDistance)
        {
            SetDestination(desiredFollowingPos, cropWalkSpeed);
        }
    }

    private void SetDestination(Vector3 desiredFollowingPos, float speed)
    {
        transform.position = Vector3.SmoothDamp(transform.position, desiredFollowingPos, ref velocityRef, 1f / speed);
    }

    private void OnDrawGizmos()
    {
        //Cambia el color del gizmo a color azul
        Gizmos.color = Color.blue;
        //Traza una linea desde la planta hacia actual del Sphere Cast.
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxHitDistance);
        //Crea la esfera visual al final de la distancia actual.
        Gizmos.DrawWireSphere(transform.position + transform.forward * maxHitDistance, sphereDetectionRadius);
    }

    //Detecta al enemigo frente 
    private bool EnemyDetection() => Physics.SphereCast(transform.position, sphereDetectionRadius, transform.forward, out hit, maxHitDistance, whatIsEnemy);

    private void LookAtPlayer()
    {
        //Consigue la direccion donde se encuentra el jugador.
        Vector3 direction = playerPos.position - transform.position;

        //Se crea un Quaternion donde se almacena la rotacion que constantemente mira al jugador.
        Quaternion lookAtPlayer = Quaternion.LookRotation(direction, Vector3.up);

        //Se crea un Quaternion nuevo donde se guardara en los grados Y la rotacion del Quaternion que ve al jugador.
        //Se crea de esta manera para evitar que la planta gire en todos sus ejes, solamente en el Y girara.
        Quaternion newQuat = Quaternion.Euler(0, lookAtPlayer.eulerAngles.y, 0);

        //Se aplica la nueva rotacion.
        transform.rotation = newQuat;

    }

    protected abstract void CropAttack();

    protected void BeginCooldownTime(float cooldownTime)
    {

    }

    #region Reference Methods
    private void GetReferences()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion
}
