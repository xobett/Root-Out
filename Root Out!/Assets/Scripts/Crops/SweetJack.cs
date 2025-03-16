using UnityEngine;

public class SweetJack : CropBase
{

    protected override void CropAttack()
    {
        base.HeadToShootingPos();

        PistolaBellota pistolaBellota = GetComponent<PistolaBellota>();
        //Hacer que dispare activando su booleano.
    }

}
