using UnityEngine;
using Weapons;

public class Lanzallamas : WeaponsBase
{
    // Duración e intensidad de la sacudida de la cámara al usar el lanzallamas
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    protected override void Shoot()
    {
        // Llama al método de la clase base para realizar el disparo
        base.Shoot();
        // Inicia la sacudida de la cámara
        CameraShake.StartShake(shakeDuration, shakeMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            // Lógica adicional cuando el lanzallamas entra en contacto con un objeto con la etiqueta "Target"
        }
    }
}


