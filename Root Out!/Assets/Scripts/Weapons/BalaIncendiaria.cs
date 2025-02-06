using System.Collections;
using UnityEngine;


public class BalaIncendiaria : MonoBehaviour, IBullet
{
    private float da�o;
    [SerializeField] private float duracion = 5f; // Duraci�n del da�o continuo
    [SerializeField] private float intervalo = 1f; // Intervalo entre cada aplicaci�n de da�o

    float tiempoTranscurrido = 0f;

    public void SetDamage(int damageAmount)
    {
        da�o = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // Aplica da�o al objetivo
           StartCoroutine(AplicarDa�oContinuo(collision.gameObject));
        }
       
    }

    private IEnumerator AplicarDa�oContinuo(GameObject objetivo)
    {

        while (tiempoTranscurrido < duracion)
        {
            // Aplica da�o al objetivo
           objetivo.GetComponent<AIHealth>().TakeDamageAI(da�o);

            // Espera el intervalo antes de aplicar el siguiente da�o
            yield return new WaitForSeconds(intervalo);
            tiempoTranscurrido += intervalo;
        }

        // Destruye la bala despu�s de que termine de aplicar el da�o continuo
        Destroy(gameObject);
    }
}
