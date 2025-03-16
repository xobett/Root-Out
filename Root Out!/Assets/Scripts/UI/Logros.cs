using System.Collections;
using TMPro;
using UnityEngine;

public class Logros : MonoBehaviour
{
    [Header("Primera Arma")]
    [SerializeField] GameObject logroCanvas;
    [SerializeField] WeaponHandler scriptWeaponHandler;
    [SerializeField] Animation animacionPrimeraArma;

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
        StartCoroutine(PrimeraArma());
        StartCoroutine(NumberEnemmiesDefeat());
    }

    public IEnumerator PrimeraArma()
    {
        if (scriptWeaponHandler.currentWeapon != null)
        {
            AudioManager.instance.PlaySFX("Logros");
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
        while (true)
        {
            enemiesDefeat = AIHealth.enemiesDefeated; // Obtener el contador de enemigos derrotado

            if (enemiesDefeat == 10)
            {
                AudioManager.instance.PlaySFX("Logros");
                animacion10Enemigos.Play();
                canvas10Enemigos.SetActive(true);
                yield return new WaitForSeconds(4f);
                animacion10Enemigos.Stop();
                canvas10Enemigos.SetActive(false);
                StopCoroutine(NumberEnemmiesDefeat());
            }

            yield return new WaitForSeconds(1f); // Actualizar cada segundo
        }
    }
}
