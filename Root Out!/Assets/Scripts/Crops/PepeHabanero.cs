using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PepeHabanero : CropBase
{
    [SerializeField] private GameObject nukePrefab;
    protected override void CropAttack()
    {
        base.HeadToEnemy();
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
            StartCoroutine(Explode(enemy.gameObject));
        }
    }

    private IEnumerator Explode(GameObject enemy)
    {
        cropAnimCtrlr.SetTrigger("Explode");

        yield return new WaitForSeconds(2);

        Instantiate(nukePrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<AIHealth>().TakeDamage(damage);

        Destroy(gameObject);
    }
}
