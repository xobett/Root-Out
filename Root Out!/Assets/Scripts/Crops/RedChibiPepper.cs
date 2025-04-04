using Unity.VisualScripting;
using UnityEngine;

public class RedChibiPepper : CropBase
{
    [Header("RED CHIBI PEPPER SETTINGS")]
    [SerializeField] private GameObject redChibiExplosion;
    protected override void CropAttack()
    {
        HeadToEnemy();
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();

        if (isFollowingEnemy)
        {
            cropAnimCtrlr.SetBool("isRunning", true);
        }
        else
        {
            cropAnimCtrlr.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Mushroom Shooter") || enemy.gameObject.CompareTag("Enemy"))
        {
            Instantiate(redChibiExplosion, transform.position, Quaternion.identity);
            enemy.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
