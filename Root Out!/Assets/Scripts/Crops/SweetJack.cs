using UnityEngine;

public class SweetJack : CropBase
{
    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
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
