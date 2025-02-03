using System.Collections;
using UnityEngine;


public class BalaIncendiaria : MonoBehaviour, IBullet
{
    private float daño;
    [SerializeField] private float duracion = 5f; // Duración del daño continuo
    [SerializeField] private float intervalo = 1f; // Intervalo entre cada aplicación de daño

    float tiempoTranscurrido = 0f;

    public void SetDamage(int damageAmount)
    {
        daño = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // Aplica daño al objetivo
           StartCoroutine(AplicarDañoContinuo(collision.gameObject));
        }
       
    }

    private IEnumerator AplicarDañoContinuo(GameObject objetivo)
    {

        while (tiempoTranscurrido < duracion)
        {
            // Aplica daño al objetivo
           objetivo.GetComponent<AIHealth>().TakeDamageAI(daño);

            // Espera el intervalo antes de aplicar el siguiente daño
            yield return new WaitForSeconds(intervalo);
            tiempoTranscurrido += intervalo;
        }

        // Destruye la bala después de que termine de aplicar el daño continuo
        Destroy(gameObject);
    }
}
