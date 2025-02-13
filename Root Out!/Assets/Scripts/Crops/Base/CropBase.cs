using UnityEngine;

public abstract class CropBase : MonoBehaviour
{
    [SerializeField] public string cropName;
    [SerializeField] protected string cropDescription;

    [SerializeField] protected CropType cropType;

    [SerializeField] protected float cropWalkSpeed;
    [SerializeField] protected float cropRunSpeed;

    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float damage;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected abstract void Attack();

    protected virtual void FollowPlayer()
    {
        Vector3 relativeDistance = player.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativeDistance, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtPlayer, 0.8f * Time.deltaTime);
        Debug.Log("Test testing");
    }

    protected void BeginCooldownTime(float cooldownTime)
    {

    }
}
