using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration = 0.5f;  // Duración del efecto de sacudida
    private float shakeMagnitude = 0.1f;  // Intensidad de la sacudida
    private float dampingSpeed = 1.0f; // Desvanecimiento de la sacudida
    private Vector3 initialPosition;  // Posición original de la cámara

    private static CameraShake instance;  // Instancia estática de la clase

    void Awake()
    {
        // Asegura que solo haya una instancia de CameraShake
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Guarda la posición original de la cámara
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Llama al método privado Shake en cada frame
        Shake();
    }

    // Método estático para iniciar la sacudida de la cámara
    public static void StartShake(float duration, float magnitude)
    {
        if (instance != null)
        {
            instance.shakeDuration = duration;
            instance.shakeMagnitude = magnitude;
        }
    }

    // Método privado para manejar la lógica de la sacudida
    private void Shake()
    {
        // Si la cámara está sacudiéndose, aplica el efecto de sacudida
        if (shakeDuration > 0)
        {
            // Genera un desplazamiento aleatorio basado en la magnitud de la sacudida
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Aplica el desplazamiento a la posición original de la cámara
            transform.localPosition = initialPosition + shakeOffset;

            // Reduce la duración de la sacudida
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Restaura la posición original de la cámara
            transform.localPosition = initialPosition;
        }
    }
}
