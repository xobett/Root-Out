using System.Collections;
using UnityEngine;

public class Logros : MonoBehaviour
{
    [Header("Primera Arma")]
    [SerializeField] GameObject logroCanvas;
    [SerializeField] WeaponHandler scriptWeaponHandler;
    [SerializeField] Animation animacionPrimeraArma;

    private bool primeraArmaLogroMostrado = false; // Variable para controlar si el logro ya se mostró
    private bool enemigosEliminadosLogroMostrado = false;

    [Header("Enemigos Derrotados")]
    [SerializeField] Animation animacion10Enemigos;
    [SerializeField] GameObject canvas10Enemigos;
    private int enemiesDefeat;

    private void Start()
    {
        animacionPrimeraArma.Stop();
        logroCanvas.SetActive(false);
        animacion10Enemigos.Stop();
        canvas10Enemigos.SetActive(false);
    }

    private void Update()
    {
        if (!primeraArmaLogroMostrado && scriptWeaponHandler.weapons.Count == 1)
        {
            StartCoroutine(PrimeraArma());
        }

        if (enemigosEliminadosLogroMostrado && AIHealth.enemiesDefeated == 10)
        {
            StartCoroutine(NumberEnemmiesDefeat());
        }

    }

    public IEnumerator PrimeraArma()
    {
        if (scriptWeaponHandler.weapons.Count == 1) // Verificar si hay exactamente una arma en scriptWeaponHandler
        {
            primeraArmaLogroMostrado = true; // Marcar el logro como mostrado

            AudioManagerSFX.Instance.PlaySFX("Logros");
            animacionPrimeraArma.Play();
            logroCanvas.SetActive(true);
            yield return new WaitForSeconds(7f);
            animacionPrimeraArma.Stop();
            logroCanvas.SetActive(false);
        }
    }

    public IEnumerator NumberEnemmiesDefeat()
    {
        if (enemiesDefeat == 10)
        {
            enemigosEliminadosLogroMostrado = true;

            AudioManagerSFX.Instance.PlaySFX("Logros");
            animacion10Enemigos.Play();
            canvas10Enemigos.SetActive(true);
            yield return new WaitForSeconds(7f);
            animacion10Enemigos.Stop();
            canvas10Enemigos.SetActive(false);
            StopCoroutine(NumberEnemmiesDefeat());
        }
    }
}
