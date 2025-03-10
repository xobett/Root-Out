using UnityEngine;

public class RedChibiPepper : CropBase
{
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
