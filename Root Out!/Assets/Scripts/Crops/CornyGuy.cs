using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CornyGuy : CropBase
{
    [Header("CORNY GUY ATTACK")]
    [SerializeField] private Vector3 rotation = new Vector3(0,3,0);

    protected override void CropAttack()
    {
        base.HeadToShootingPos();
        ShootAround();
    }

    private void ShootAround()
    {
        //Aqui rota constantemente.
        transform.Rotate(rotation);

        PistolaBellota pistolaBellota = GetComponent<PistolaBellota>();
        //Hacer que dispare activando su booleano.

        Destroy(gameObject, 10);
    }
}
