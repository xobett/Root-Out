using UnityEngine;

public class PepeHabanero : CropBase
{
    [SerializeField] private GameObject nukePrefab;
    protected override void CropAttack()
    {
        HeadToEnemy();
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
    }

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Mushroom Shooter") || enemy.gameObject.CompareTag("Enemy"))
        {
            enemy.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
            Instantiate(nukePrefab, transform.position, Quaternion.identity);

            //Destroy nuke after animation 

            Destroy(gameObject);
        }
    }
}
