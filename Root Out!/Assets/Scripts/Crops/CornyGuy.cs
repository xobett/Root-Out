using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CornyGuy : CropBase
{
    [Header("CORNY GUY ATTACK")]
    [SerializeField] private Transform shootSpawn;
    [SerializeField] private Vector3 rotation = new Vector3(0, 3, 0);

    protected override void CropAttack()
    {
        //base.HeadToShootingPos();
        //ShootAround();
        base.HeadToShootingPos();
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
    }

    private void ShootAround()
    {
        //Aqui rota constantemente.
        shootSpawn.transform.Rotate(rotation);

        GetComponent<WeaponCoryGuy>().enabled = true;

        Destroy(gameObject, 10);
    }
}
