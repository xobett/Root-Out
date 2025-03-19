using Unity.VisualScripting;
using UnityEngine;

public class RedChibiPepper : CropBase
{
    protected override void Update()
    {
        base.Update();
        SetAnimatorParameters();

    }

    protected override void SetAnimatorParameters()
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

    protected override void CropAttack()
    {
        HeadToEnemy();
    }

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Mushroom Shooter") || enemy.gameObject.CompareTag("Enemy"))
        {
            enemy.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
