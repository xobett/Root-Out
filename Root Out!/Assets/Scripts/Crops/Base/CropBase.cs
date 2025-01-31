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

    protected abstract void Attack();

    protected void BeginCooldownTime(float cooldownTime)
    {

    }
}
