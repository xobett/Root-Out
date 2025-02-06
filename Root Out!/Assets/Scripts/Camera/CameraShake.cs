using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration = 0.5f;  // Duraci�n del efecto de sacudida
    private float shakeMagnitude = 0.1f;  // Intensidad de la sacudida
    private float dampingSpeed = 1.0f; // Desvanecimiento de la sacudida
    private Vector3 initialPosition;  // Posici�n original de la c�mara

    private static CameraShake instance;  // Instancia est�tica de la clase

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
        // Guarda la posici�n original de la c�mara
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Llama al m�todo privado Shake en cada frame
        Shake();
    }

    // M�todo est�tico para iniciar la sacudida de la c�mara
    public static void StartShake(float duration, float magnitude)
    {
        if (instance != null)
        {
            instance.shakeDuration = duration;
            instance.shakeMagnitude = magnitude;
        }
    }

    // M�todo privado para manejar la l�gica de la sacudida
    private void Shake()
    {
        // Si la c�mara est� sacudi�ndose, aplica el efecto de sacudida
        if (shakeDuration > 0)
        {
            // Genera un desplazamiento aleatorio basado en la magnitud de la sacudida
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Aplica el desplazamiento a la posici�n original de la c�mara
            transform.localPosition = initialPosition + shakeOffset;

            // Reduce la duraci�n de la sacudida
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Restaura la posici�n original de la c�mara
            transform.localPosition = initialPosition;
        }
    }
}
