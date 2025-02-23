
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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found");
    }

    private void Update()
    {
        FollowPlayer();
    }

    protected abstract void Attack();

    protected virtual void FollowPlayer()
    {
        //LookAtPlayer();

        Vector3 desiredFollowingPos = player.transform.position + followingOffset;
        desiredFollowingPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, desiredFollowingPos);

        Debug.Log(distance);

        if (distance > 1)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredFollowingPos, ref velocityRef, 1f / cropWalkSpeed);
            //Debug.Log($"Moving and desired vector is: {desiredFollowingPos}");
        }


        //Debug.Log(desiredFollowingPos);


    }

    private void LookAtPlayer()
    {
        Vector3 relativeDistance = player.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativeDistance, Vector3.up);

        Quaternion testAngle = Quaternion.Euler(0, lookAtPlayer.y, 0);

        float desiredRotation = Mathf.Lerp(transform.eulerAngles.y, lookAtPlayer.eulerAngles.y, lookAtPlayerSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, desiredRotation, 0);


    }

    protected void BeginCooldownTime(float cooldownTime)
    {

    }
}
