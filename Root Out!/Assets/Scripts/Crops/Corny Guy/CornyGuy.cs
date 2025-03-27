using System.Collections;
using UnityEngine;

public class CornyGuy : CropBase
{
    [Header("CORNY GUY ATTACK")]
    [SerializeField] private float timeBeforeShooting;
    [SerializeField] private float shootingTime;
    [SerializeField] private float nudeTime;
    [SerializeField] private float unNudeTransitionTime;

    [SerializeField] private Transform shootPivot;
    [SerializeField] private Transform shootSpawn;

    [SerializeField] private Vector3 rotation = new Vector3(0, 3, 0);

    [SerializeField] protected bool arrivedToShootingPosition;
    [SerializeField] private bool ableToShoot;

    [SerializeField] private bool abilityUsed;

    protected override void CropAttack()
    {
        HeadToShootingPos();
        ShootAround();
    }

    protected override void HeadToShootingPos()
    {
        if (enemyPos != null)
        {
            LookAtTarget(enemyPos);
            Vector3 desiredShootingPos = enemyPos.transform.position + enemyPos.transform.forward * frontShootingDistance + enemyPos.transform.right * sideShootingDistance;
            desiredShootingPos.y = transform.position.y;

            float distance = Vector3.Distance(transform.position, desiredShootingPos);

            if (distance > 1 && !arrivedToShootingPosition)
            {
                SetDestination(desiredShootingPos, cropRunSpeed);
            }
            else
            {
                arrivedToShootingPosition = true;
            }
        }
        else if (enemyPos == null && !abilityUsed)
        {
            enemyDetected = false;
        }
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
    }

    private void ShootAround()
    {
        if (arrivedToShootingPosition && !abilityUsed)
        {
            StartCoroutine(ShootCorns());
        }

        if (ableToShoot)
        {
            shootPivot.transform.Rotate(rotation);
        }

        //Destroy(gameObject, 10);
    }

    private IEnumerator ShootCorns()
    {
        abilityUsed = true;

        Debug.Log("Entered the ability method");

        yield return new WaitUntil(() => arrivedToShootingPosition);

        cropAnimCtrlr.SetTrigger("Shoot Corns");

        yield return new WaitForSeconds(timeBeforeShooting);

        ableToShoot = true;
        GetComponent<WeaponCoryGuy>().enabled = true;

        yield return new WaitForSeconds(shootingTime);

        ableToShoot = false;
        GetComponent<WeaponCoryGuy>().enabled = false;

        cropAnimCtrlr.SetBool("isNude", true);

        yield return new WaitForSeconds(nudeTime);

        cropAnimCtrlr.SetBool("isNude", false);

        yield return new WaitForSeconds(unNudeTransitionTime);

        arrivedToShootingPosition = false;
        enemyPos = null;
        abilityUsed = false;
    }
}
