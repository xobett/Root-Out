
using UnityEngine;

public abstract class CropBase : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField] public string cropName;
    [SerializeField] protected string cropDescription;

    [SerializeField] protected CropType cropType;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField, Range(0f, 1f)] protected float cropWalkSpeed;
    [SerializeField] protected float cropRunSpeed;

    [SerializeField, Range(0.5f, 10f)] protected float lookAtPlayerSpeed;

    [Header("FOLLOW SETTINGS")]
    [SerializeField, Range(0,5)] protected float stoppingDistance;
    [SerializeField] protected float backDistance;
    [SerializeField] protected float sideDistance;
    private Vector3 velocityRef = Vector3.zero;

    [Header("COMBAT SETTINGS")]
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float damage;

    private GameObject player;

    private float turnSmooth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found");
    }

    private void Update()
    {
        FollowPlayer();
    }

    protected abstract void Ability();

    protected virtual void FollowPlayer()
    {
        LookAtPlayer();

        Vector3 desiredFollowingPos = player.transform.position + player.transform.forward * -backDistance + transform.right * sideDistance;
        desiredFollowingPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        if (distance > stoppingDistance)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredFollowingPos, ref velocityRef, 1f / cropWalkSpeed);
        }
    }

    private void LookAtPlayer()
    {
        //Gets direction to face
        Vector3 direction = player.transform.position - transform.position;

        //Creates a quaternion that will look at the direction set.
        Quaternion lookAtPlayer = Quaternion.LookRotation(direction, Vector3.up);

        Quaternion newQuat = Quaternion.Euler(0, lookAtPlayer.eulerAngles.y, 0);

        float newAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        //Debug.Log(newAngle);


        //Sets the new rotation.
        transform.rotation = newQuat;

    }

    protected void BeginCooldownTime(float cooldownTime)
    {

    }
}
