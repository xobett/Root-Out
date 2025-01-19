using UnityEngine;
using Weapons;

public class Lanzallamas : WeaponsBase
{
    // Duraci�n e intensidad de la sacudida de la c�mara al usar el lanzallamas
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    protected override void Shoot()
    {
        // Llama al m�todo de la clase base para realizar el disparo
        base.Shoot();
        // Inicia la sacudida de la c�mara
        CameraShake.StartShake(shakeDuration, shakeMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            // L�gica adicional cuando el lanzallamas entra en contacto con un objeto con la etiqueta "Target"
        }
    }
}


