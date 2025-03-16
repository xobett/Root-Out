using System.Collections;
using Unity.VisualScripting;
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
        if (!primeraArmaLogroMostrado)
        {
            AudioManager.instance.PlaySFX("Logros");
            StartCoroutine(PrimeraArma());
        }
        StartCoroutine(NumberEnemmiesDefeat());

        animacionPrimeraArma.Stop();
        logroCanvas.SetActive(false);
        animacion10Enemigos.Stop();
        canvas10Enemigos.SetActive(false);
    }
    
    private void Update()
    {
       
    }

    public IEnumerator PrimeraArma()
    {
        if (scriptWeaponHandler.weaponPrefabs.Count == 0) // Verificar si hay exactamente una arma en scriptWeaponHandler
        {
            animacionPrimeraArma.Play();
            logroCanvas.SetActive(true);
            yield return new WaitForSeconds(4f);
            animacionPrimeraArma.Stop();
            logroCanvas.SetActive(false);
            primeraArmaLogroMostrado = true; // Marcar el logro como mostrado
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
