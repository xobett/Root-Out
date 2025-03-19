using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CornyGuy : CropBase
{
    [Header("CORNY GUY ATTACK")]
    [SerializeField] private Vector3 rotation = new Vector3(0, 3, 0);

    protected override void CropAttack()
    {
        //base.HeadToShootingPos();
        ShootAround();
    }

    protected override void SetAnimatorParameters()
    {
        throw new System.NotImplementedException();
    }

    private void ShootAround()
    {
        //Aqui rota constantemente.
        transform.Rotate(rotation);

        if (TryGetComponent<WeaponCoryGuy>(out WeaponCoryGuy weaponCoryGuy))
        {
            weaponCoryGuy.enabled = true;
            Debug.Log("Arma activada");
        }

        Destroy(gameObject, 10);
    }
}
