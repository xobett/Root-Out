using System.Collections;
using UnityEngine;
using System;



public class BalaIncendiaria : MonoBehaviour, IBullet
{
    private float damage;
    [SerializeField] private float duracion = 5f; // Duraci�n del da�o continuo
    [SerializeField] private float intervalo = 1f; // Intervalo entre cada aplicaci�n de da�o

    float tiempoTranscurrido = 0f;

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
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
           objetivo.GetComponent<AIHealth>().TakeDamage(damage);

            // Espera el intervalo antes de aplicar el siguiente da�o
            yield return new WaitForSeconds(intervalo);
            tiempoTranscurrido += intervalo;
            yield return null;
        }

        // Destruye la bala despu�s de que termine de aplicar el da�o continuo
        Destroy(gameObject);
    }
}
