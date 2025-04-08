using System.Collections;
using UnityEngine;

public class Logros : MonoBehaviour
{
    [Header("Primera Arma")]
    [SerializeField] GameObject logroCanvas;
    [SerializeField] WeaponHandler scriptWeaponHandler;
    [SerializeField] Animation animacionPrimeraArma;
    private bool primeraArmaLogroMostrado = false; // Variable para controlar si el logro ya se mostró

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
        if (!primeraArmaLogroMostrado)
        {
            StartCoroutine(PrimeraArma());
        }
        StartCoroutine(NumberEnemmiesDefeat());
    }

    public IEnumerator PrimeraArma()
    {
        if (scriptWeaponHandler.weapons.Count == 1) // Verificar si hay exactamente una arma en scriptWeaponHandler
        {
            AudioManagerSFX.Instance.PlaySFX("Logros");
            primeraArmaLogroMostrado = true; // Marcar el logro como mostrado
            animacionPrimeraArma.Play();
            logroCanvas.SetActive(true);
            yield return new WaitForSeconds(7f);
            animacionPrimeraArma.Stop();
            logroCanvas.SetActive(false);
        }
    }

    public IEnumerator NumberEnemmiesDefeat()
    {
        while (true)
        {
            enemiesDefeat = AIHealth.enemiesDefeated; // Obtener el contador de enemigos derrotado

            if (enemiesDefeat == 10)
            {
                AudioManagerSFX.Instance.PlaySFX("Logros");
                animacion10Enemigos.Play();
                canvas10Enemigos.SetActive(true);
                yield return new WaitForSeconds(7f);
                animacion10Enemigos.Stop();
                canvas10Enemigos.SetActive(false);
                StopCoroutine(NumberEnemmiesDefeat());
            }

            yield return new WaitForSeconds(1f); // Actualizar cada segundo
            yield return null;
        }
    }
}
