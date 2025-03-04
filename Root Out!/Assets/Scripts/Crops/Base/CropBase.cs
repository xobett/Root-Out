
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

    [SerializeField, Range(0.5f,10f)] protected float lookAtPlayerSpeed;

    [SerializeField] protected Vector3 followingOffset; 
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

        //Vector3 desiredFollowingPos = player.transform.position + followingOffset;
        //desiredFollowingPos.y = transform.position.y;

        //float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        //if (distance > 1)
        //{
        //    transform.position = Vector3.SmoothDamp(transform.position, desiredFollowingPos, ref velocityRef, 1f / cropWalkSpeed);
        //}
    }

    private void LookAtPlayer()
    {
        //Gets direction to face
        Vector3 direction = player.transform.position - transform.position;

        //Creates a quaternion that will look at the direction set.
        Quaternion lookAtPlayer = Quaternion.LookRotation(direction, Vector3.up);

        //Lerps the current Y rotation to the quaternion that will look at the player.
        float desiredRotation = Mathf.Lerp(transform.eulerAngles.y, lookAtPlayer.eulerAngles.y, lookAtPlayerSpeed * Time.deltaTime);

        //Sets the new rotation.
        transform.rotation = Quaternion.Euler(0, desiredRotation, 0);

        float newAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        Debug.Log(newAngle);

    }

    protected void BeginCooldownTime(float cooldownTime)
    {

    }
}
