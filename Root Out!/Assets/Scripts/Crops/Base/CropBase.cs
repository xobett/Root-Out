using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class CropBase : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField] protected CropData cropData;

    [Header("ANIMATION SETTINGS")]
    [SerializeField] protected Animator cropAnimCtrlr;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField, Range(0f, 1f)] protected float cropWalkSpeed;
    [SerializeField, Range(0f, 1f)] protected float cropRunSpeed;

    protected float originalRunSpeed;

    [Header("FOLLOW SETTINGS")]
    [SerializeField, Range(0, 5)] protected float stoppingDistance;
    [SerializeField] protected float backDistance;
    [SerializeField] protected float sideDistance;

    protected Transform playerPos;

    protected Vector3 velocityRef = Vector3.zero;

    [Header("COMBAT SETTINGS")]
    [SerializeField] protected float damage;
    [SerializeField] protected float frontShootingDistance;
    [SerializeField] protected float sideShootingDistance;


    [Header("ENEMY DETECTION")]
    protected float sphereDetectionRadius = 15f;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float maxHitDistance;

    [SerializeField] protected bool arrivedToShootPos;
    [SerializeField] protected bool enemyDetected;
    [SerializeField] protected bool isFollowingPlayer;
    [SerializeField] protected bool isFollowingEnemy;

    protected RaycastHit hit;

    protected Transform enemyPos;

    protected CropHandler cropHandler;

    private void Start()
    {
        GetReferences();

        originalRunSpeed = cropRunSpeed;
    }

    protected virtual void Update()
    {
        BehaviourCheck();
        SetAnimatorParameters();

        Collider[] enemyColliders = Physics.OverlapSphere(playerPos.position, sphereDetectionRadius, whatIsEnemy);

        foreach(Collider enemyCollider in enemyColliders)
        {
            Debug.Log("Enemy detected");
        }
    }

    private void FixedUpdate()
    {
        //Para alcanzar al enemigo eventualmente, se suma un valor chico a la velocidad de movimiento progresivamente.
        if (enemyDetected && enemyPos != null)
        {
            cropRunSpeed += 0.01f;
        } 
    }

    protected void BehaviourCheck()
    {
        if (GetEnemyInWorld() && !enemyDetected)
        {
            isFollowingPlayer = false;
            enemyDetected = true;
            enemyPos = GetEnemyInWorld().transform;
        }

        //if (EnemyDetection() && !enemyDetected)
        //{
        //    isFollowingPlayer = false;
        //    enemyDetected = true;
        //    enemyPos = hit.collider.transform;
        //}

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
        //En caso de que otra planta haya atacado y eliminado primero al enemigo a seguir, se checa que no sea nulo.
        if (enemyPos != null)
        {
            LookAtTarget(enemyPos);
            Vector3 desiredFollowingPos = enemyPos.position;
            desiredFollowingPos.y = transform.position.y;

            SetDestination(desiredFollowingPos, cropRunSpeed);
            isFollowingEnemy = true;
        }
        else
        {
            isFollowingEnemy = false;
            enemyDetected = false;
        }
    }

    protected virtual void HeadToPlayer()
    {
        //Se resetea la velocidad de la planta cuando vuelve a seguir al jugador.
        cropRunSpeed = originalRunSpeed;

        LookAtTarget(playerPos);

        Vector3 desiredFollowingPos = playerPos.position + playerPos.forward * -backDistance + transform.right * sideDistance;
        desiredFollowingPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        if (distance > stoppingDistance)
        {
            isFollowingPlayer = true;
            SetDestination(desiredFollowingPos, cropWalkSpeed);
        }
        else
        {
            isFollowingPlayer = false;
        }
    }
    
    protected virtual void HeadToShootingPos()
    {
        if (enemyPos != null)
        {
            LookAtTarget(enemyPos);
            Vector3 desiredShootingPos = enemyPos.transform.position + enemyPos.transform.forward * frontShootingDistance + enemyPos.transform.right * sideShootingDistance;
            desiredShootingPos.y = transform.position.y;

            float distance = Vector3.Distance(transform.position, desiredShootingPos);

            if (distance > 1)
            {
                arrivedToShootPos = false;
                SetDestination(desiredShootingPos, cropRunSpeed);
            }
            else
            {
                arrivedToShootPos = true;
            }
        }
        else
        {
            enemyDetected = false;
        }
    }

    protected void SetDestination(Vector3 desiredFollowingPos, float speed)
    {
        //Moves at a constant speed.
        transform.position = Vector3.MoveTowards(transform.position, desiredFollowingPos, Time.deltaTime * 2.8f);
    }
    protected void LookAtTarget(Transform target)
    {
        //Consigue la direccion donde se encuentra el jugador.
        Vector3 direction = target.position - transform.position;

        //Se crea un Quaternion donde se almacena la rotacion que constantemente mira al jugador.
        Quaternion lookAtTarget = Quaternion.LookRotation(direction, Vector3.up);

        //Se crea un Quaternion nuevo donde se guardara en los grados Y la rotacion del Quaternion que ve al jugador.
        //Se crea de esta manera para evitar que la planta gire en todos sus ejes, solamente en el Y girara.
        Quaternion lookRotation = Quaternion.Euler(0, lookAtTarget.eulerAngles.y, 0);

        //Se aplica la nueva rotacion.
        transform.rotation = lookRotation;

    }
    protected abstract void CropAttack();
    protected virtual void SetAnimatorParameters()
    {
        if (isFollowingPlayer)
        {
            cropAnimCtrlr.SetBool("isWalking", true);
        }
        else
        {
            cropAnimCtrlr.SetBool("isWalking", false);
        }
    }

    //Detecta al enemigo frente 
    private bool EnemyDetection() => Physics.SphereCast(transform.position, sphereDetectionRadius, transform.forward, out hit, maxHitDistance, whatIsEnemy);

    public GameObject GetEnemyInWorld()
    {
        GameObject enemyToFollow;

        Collider[] enemyColliders = Physics.OverlapSphere(playerPos.position, sphereDetectionRadius, whatIsEnemy);

        if (enemyColliders != null && enemyColliders.Length > 1)
        {
            enemyToFollow = enemyColliders[Random.Range(0, enemyColliders.Length)].gameObject;
        }
        else
        {
            enemyToFollow = GameObject.FindGameObjectWithTag("Enemy");
        }

        return enemyToFollow;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (playerPos != null)
        {
            Gizmos.DrawWireSphere(playerPos.position, sphereDetectionRadius); 
        }
    }

    private void OnDestroy()
    {
        cropHandler.UpdateDroppedCrop(cropData.CropName);
    }

    #region Reference Methods
    private void GetReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerPos = player.transform;
        cropHandler = player.GetComponent<CropHandler>();
    }
    #endregion
}
