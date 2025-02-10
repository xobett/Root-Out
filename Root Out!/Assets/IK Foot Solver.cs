using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask capaTerreno = default; // M�scara de capa para detectar el terreno
    [SerializeField] Transform cuerpo = default; // Transform del cuerpo principal
    [SerializeField] IKFootSolver otroPie = default; // Referencia al otro pie
    [SerializeField] float velocidad = 1f; // Velocidad del paso
    [SerializeField] float distanciaPaso = 4f; // Distancia m�nima para dar un paso
    [SerializeField] float longitudPaso = 4f; // Longitud del paso
    [SerializeField] float alturaPaso = 1f; // Altura del paso
    [SerializeField] Vector3 desplazamientoPie = default; // Desplazamiento del pie desde la posici�n original

    float espacioPie; // Espacio inicial del pie
    Vector3 posicionAntigua, posicionActual, nuevaPosicion; // Posiciones del pie
    Vector3 normalAntigua, normalActual, nuevaNormal; // Normales del pie
    float interpolacion; // Variable de interpolaci�n para el paso

    private void Start()
    {
        espacioPie = transform.localPosition.x; // Inicializa el espacio del pie
        posicionActual = nuevaPosicion = posicionAntigua = transform.position; // Inicializa las posiciones del pie
        normalActual = nuevaNormal = normalAntigua = transform.up; // Inicializa las normales del pie
        interpolacion = 1; // Inicializa la interpolaci�n
    }

    void Update()
    {
        transform.position = posicionActual; // Actualiza la posici�n del pie
        transform.up = normalActual; // Actualiza la normal del pie

        Ray rayo = new(cuerpo.position + (cuerpo.right * espacioPie), Vector3.down); // Crea un rayo desde la posici�n del cuerpo hacia abajo

        // Si el rayo impacta en el terreno
        if (Physics.Raycast(rayo, out RaycastHit informacion, 10, capaTerreno.value))
        {
            // Si la distancia a la nueva posici�n es mayor que la distancia del paso y el otro pie no se est� moviendo y la interpolaci�n ha terminado
            if (Vector3.Distance(nuevaPosicion, informacion.point) > distanciaPaso && !otroPie.EstaMoviendose() && interpolacion >= 1)
            {
                interpolacion = 0; // Resetea la interpolaci�n
                int direccion = cuerpo.InverseTransformPoint(informacion.point).z > cuerpo.InverseTransformPoint(nuevaPosicion).z ? 1 : -1; // Determina la direcci�n del paso
                nuevaPosicion = informacion.point + (cuerpo.forward * longitudPaso * direccion) + desplazamientoPie; // Calcula la nueva posici�n del pie
                nuevaNormal = informacion.normal; // Actualiza la normal del pie
            }
        }

        // Si la interpolaci�n no ha terminado
        if (interpolacion < 1)
        {
            Vector3 tempPosicion = Vector3.Lerp(posicionAntigua, nuevaPosicion, interpolacion); // Interpola entre la posici�n antigua y la nueva
            tempPosicion.y += Mathf.Sin(interpolacion * Mathf.PI) * alturaPaso; // Ajusta la altura del paso

            posicionActual = tempPosicion; // Actualiza la posici�n actual del pie
            normalActual = Vector3.Lerp(normalAntigua, nuevaNormal, interpolacion); // Interpola entre las normales
            interpolacion += Time.deltaTime * velocidad; // Incrementa la interpolaci�n
        }
        else
        {
            posicionAntigua = nuevaPosicion; // Actualiza la posici�n antigua
            normalAntigua = nuevaNormal; // Actualiza la normal antigua
        }
    }

    // M�todo para dibujar Gizmos en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Establece el color del Gizmo
        Gizmos.DrawSphere(nuevaPosicion, 0.5f); // Dibuja una esfera en la nueva posici�n del pie
    }

    // M�todo para verificar si el pie se est� moviendo
    public bool EstaMoviendose()
    {
        return interpolacion < 1; // Retorna true si la interpolaci�n no ha terminado
    }
}

