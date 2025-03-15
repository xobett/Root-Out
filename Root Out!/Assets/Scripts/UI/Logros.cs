using System.Collections;
using UnityEngine;

public class Logros : MonoBehaviour
{
    [SerializeField] private WeaponHandler weaponHandler;
    [SerializeField] Animation primeraArma;
    [SerializeField] Animation logoXP;


    private void Start()
    {
        primeraArma.Stop();
        logoXP.Stop();
    }
    private void Update()
    {
       StartCoroutine(PrimeraArma());
    }

    public IEnumerator PrimeraArma()
    {
        if (weaponHandler != null && weaponHandler.currentWeapon != null)
        {
            primeraArma.Play();
            logoXP.Play();
            yield return new WaitForSeconds(4f);
            primeraArma.Stop(); 
            logoXP.Stop();
        }
    }
}
