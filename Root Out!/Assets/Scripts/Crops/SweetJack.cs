using Unity.VisualScripting;
using UnityEngine;

public class SweetJack : CropBase
{
    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();

        if (enemyDetected && !arrivedToShootPos)
        {
            cropAnimCtrlr.SetBool("isRunning", true);
        }
        else if (enemyDetected && arrivedToShootPos || enemyPos == null)
        {
            cropAnimCtrlr.SetBool("isRunning", false);
        }
    }

    protected override void HeadToPlayer()
    {
        base.HeadToPlayer();
        GetComponent<WeaponCoryGuy>().enabled = false;
    }

    protected override void CropAttack()
    {
        base.HeadToShootingPos();

        GetComponent<WeaponCoryGuy>().enabled = true;
    }

}
