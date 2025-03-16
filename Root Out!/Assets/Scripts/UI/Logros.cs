using System.Collections;
using UnityEngine;

public class Logros : MonoBehaviour
{
    [Header("Primera Arma")]
    [SerializeField] GameObject logroCanvas;
    [SerializeField] WeaponHandler scriptWeaponHandler;
    [SerializeField] Animation animacionPrimeraArma;

    [Header("Enemigos Derrotados")]
    [SerializeField] int enemiesDefeat;
    [SerializeField] Animation animacion10Enemigos;
    [SerializeField] GameObject canvas10Enemigos;

    private void Start()
    {
        animacionPrimeraArma.Stop();
        logroCanvas.SetActive(false);
        animacion10Enemigos.Stop();
        canvas10Enemigos.SetActive(false);
    }

    private void Update()
    {
        StartCoroutine(PrimeraArma());
        StartCoroutine(NumberEnemmiesDefeat());
    }

    public IEnumerator PrimeraArma()
    {
        if (scriptWeaponHandler.currentWeapon != null)
        {
            animacionPrimeraArma.Play();
            logroCanvas.SetActive(true);
            yield return new WaitForSeconds(4f);
            animacionPrimeraArma.Stop();
            logroCanvas.SetActive(false);
            StopCoroutine(PrimeraArma());
        }
    }

    public IEnumerator NumberEnemmiesDefeat()
    {
        enemiesDefeat = AIHealth.enemiesDefeated; // Obtiene el contador de enemigos derrotados
        if (enemiesDefeat == 10)
        {
            animacion10Enemigos.Play();
            canvas10Enemigos.SetActive(true);
            yield return new WaitForSeconds(4f);
            animacion10Enemigos.Stop();
            canvas10Enemigos.SetActive(false);
            StopCoroutine(NumberEnemmiesDefeat());
        }
    }
}
