using UnityEngine;

public class PepeHabanero : CropBase
{
    protected override void CropAttack()
    {
        HeadToEnemy();
    }

    protected override void SetAnimatorParameters()
    {
        throw new System.NotImplementedException();
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
