using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask capaTerreno = default; // Máscara de capa para detectar el terreno
    [SerializeField] Transform cuerpo = default; // Transform del cuerpo principal
    [SerializeField] IKFootSolver otroPie = default; // Referencia al otro pie
    [SerializeField] float velocidad = 1f; // Velocidad del paso
    [SerializeField] float distanciaPaso = 4f; // Distancia mínima para dar un paso
    [SerializeField] float longitudPaso = 4f; // Longitud del paso
    [SerializeField] float alturaPaso = 1f; // Altura del paso
    [SerializeField] Vector3 desplazamientoPie = default; // Desplazamiento del pie desde la posición original

    float espacioPie; // Espacio inicial del pie
    Vector3 posicionAntigua, posicionActual, nuevaPosicion; // Posiciones del pie
    Vector3 normalAntigua, normalActual, nuevaNormal; // Normales del pie
    float interpolacion; // Variable de interpolación para el paso

    private void Start()
    {
        espacioPie = transform.localPosition.x; // Inicializa el espacio del pie
        posicionActual = nuevaPosicion = posicionAntigua = transform.position; // Inicializa las posiciones del pie
        normalActual = nuevaNormal = normalAntigua = transform.up; // Inicializa las normales del pie
        interpolacion = 1; // Inicializa la interpolación
    }

    void Update()
    {
        transform.position = posicionActual; // Actualiza la posición del pie
        transform.up = normalActual; // Actualiza la normal del pie

        Ray rayo = new(cuerpo.position + (cuerpo.right * espacioPie), Vector3.down); // Crea un rayo desde la posición del cuerpo hacia abajo

        // Si el rayo impacta en el terreno
        if (Physics.Raycast(rayo, out RaycastHit informacion, 10, capaTerreno.value))
        {
            // Si la distancia a la nueva posición es mayor que la distancia del paso y el otro pie no se está moviendo y la interpolación ha terminado
            if (Vector3.Distance(nuevaPosicion, informacion.point) > distanciaPaso && !otroPie.EstaMoviendose() && interpolacion >= 1)
            {
                interpolacion = 0; // Resetea la interpolación
                int direccion = cuerpo.InverseTransformPoint(informacion.point).z > cuerpo.InverseTransformPoint(nuevaPosicion).z ? 1 : -1; // Determina la dirección del paso
                nuevaPosicion = informacion.point + (cuerpo.forward * longitudPaso * direccion) + desplazamientoPie; // Calcula la nueva posición del pie
                nuevaNormal = informacion.normal; // Actualiza la normal del pie
            }
        }

        // Si la interpolación no ha terminado
        if (interpolacion < 1)
        {
            Vector3 tempPosicion = Vector3.Lerp(posicionAntigua, nuevaPosicion, interpolacion); // Interpola entre la posición antigua y la nueva
            tempPosicion.y += Mathf.Sin(interpolacion * Mathf.PI) * alturaPaso; // Ajusta la altura del paso

            posicionActual = tempPosicion; // Actualiza la posición actual del pie
            normalActual = Vector3.Lerp(normalAntigua, nuevaNormal, interpolacion); // Interpola entre las normales
            interpolacion += Time.deltaTime * velocidad; // Incrementa la interpolación
        }
        else
        {
            posicionAntigua = nuevaPosicion; // Actualiza la posición antigua
            normalAntigua = nuevaNormal; // Actualiza la normal antigua
        }
    }

    // Método para dibujar Gizmos en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Establece el color del Gizmo
        Gizmos.DrawSphere(nuevaPosicion, 0.5f); // Dibuja una esfera en la nueva posición del pie
    }

    // Método para verificar si el pie se está moviendo
    public bool EstaMoviendose()
    {
        return interpolacion < 1; // Retorna true si la interpolación no ha terminado
    }
}

