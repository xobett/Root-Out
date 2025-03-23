using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CornyGuy : CropBase
{
    [Header("CORNY GUY ATTACK")]
    [SerializeField] private Transform shootPivot;
    [SerializeField] private Vector3 rotation = new Vector3(0, 3, 0);

    [SerializeField] protected bool arrivedToShootingPosition;

    protected override void CropAttack()
    {
        ShootAround();
        HeadToShootingPos();
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
        else
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
        if (arrivedToShootingPosition)
        {
            shootPivot.transform.Rotate(rotation);

            GetComponent<WeaponCoryGuy>().enabled = true; 
        }

        //Destroy(gameObject, 10);
    }
}
