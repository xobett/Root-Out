using UnityEngine;


public class Walker : MonoBehaviour
{
    [SerializeField] AnimationCurve horizontalCurve; // Curva de animación para el movimiento horizontal del pie
    [SerializeField] AnimationCurve verticalCurve; // Curva de animación para el movimiento vertical del pie
    [SerializeField] Transform rightFootTarget1; // Transform del objetivo del pie derecho
    [SerializeField] Transform leftFootTarget1; // Transform del objetivo del pie izquierdo

    Vector3 rigthOffsetTarget1; // Desplazamiento inicial del pie derecho
    Vector3 leftOffsetTarget1; // Desplazamiento inicial del pie izquierdo

    float rightLegForwadMovement; // Movimiento hacia adelante del pie derecho
    float leftLegForwadMovement; // Movimiento hacia adelante del pie izquierdo

    float rightLegLast; // Última posición hacia adelante del pie derecho
    float leftLegLast; // Última posición hacia adelante del pie izquierdo

    RaycastHit hit; // Información del impacto del rayo

    Vector3 lastPosition; // Última posición del personaje

    float timeOffset = 0.5f; // Desplazamiento de tiempo para desfasar los pasos

    private void Start()
    {
        // Inicializar el desplazamiento del pie derecho e izquierdo a sus posiciones locales actuales
        rigthOffsetTarget1 = rightFootTarget1.localPosition;
        leftOffsetTarget1 = leftFootTarget1.localPosition;

        // Inicializar la última posición del personaje a su posición actual
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Si el personaje se está moviendo, realizar la caminata procedural
        if (IsMoving())
        {
            ProceduralWalk();
        }

        // Actualizar la última posición del personaje a su posición actual
        lastPosition = transform.position;
    }

    void ProceduralWalk()
    {
        // Ejecutar los movimientos de las piernas y la detección de colisiones
        LeftWalk();
        RightWalk();
        LeftDetection();
        RightDetection();
    }

    void RightWalk()
    {
        // Evaluar la curva de animación para el pie derecho con un desplazamiento de tiempo
        float curveTime = (Time.time + timeOffset) % 1f;
        rightLegForwadMovement = horizontalCurve.Evaluate(curveTime);

        // Actualizar la posición local del pie derecho basado en las curvas de animación horizontal y vertical
        rightFootTarget1.localPosition = rigthOffsetTarget1 +
            this.transform.forward * rightLegForwadMovement + // Corregir dirección hacia adelante
            this.transform.up * verticalCurve.Evaluate(curveTime);

        // Actualizar la última posición hacia adelante del pie derecho
        rightLegLast = rightLegForwadMovement;
    }

    void LeftWalk()
    {
        // Evaluar la curva de animación para el pie izquierdo sin desplazamiento de tiempo
        float curveTime = Time.time % 1f;
        leftLegForwadMovement = horizontalCurve.Evaluate(curveTime);

        // Actualizar la posición local del pie izquierdo basado en las curvas de animación horizontal y vertical
        leftFootTarget1.localPosition = leftOffsetTarget1 +
            this.transform.forward * leftLegForwadMovement + // Corregir dirección hacia adelante
            this.transform.up * verticalCurve.Evaluate(curveTime);

        // Actualizar la última posición hacia adelante del pie izquierdo
        leftLegLast = leftLegForwadMovement;
    }

    void RightDetection()
    {
        // Calcular la dirección del movimiento del pie derecho
        float rightLegDirectional = rightLegForwadMovement - rightLegLast;

        // Si el pie derecho se está moviendo hacia adelante y detecta una colisión, actualizar la posición del pie derecho
        if (rightLegDirectional > 0 && RightSidedRay())
        {
            rightFootTarget1.position = hit.point;
        }
    }

    void LeftDetection()
    {
        // Calcular la dirección del movimiento del pie izquierdo
        float leftLegDirectional = leftLegForwadMovement - leftLegLast;

        // Si el pie izquierdo se está moviendo hacia atrás y detecta una colisión, actualizar la posición del pie izquierdo
        if (leftLegDirectional < 0 && LeftSidedRay())
        {
            leftFootTarget1.position = hit.point;
        }
    }

    bool IsMoving()
    {
        // Comparar la posición actual con la última posición para determinar si el personaje se está moviendo
        return Vector3.Distance(transform.position, lastPosition) > 0.01f; // Ajustar el umbral según sea necesario
    }

    bool LeftSidedRay()
    {
        // Enviar un rayo desde la posición del pie izquierdo hacia abajo y retornar si detecta una colisión
        return Physics.Raycast(leftFootTarget1.position + leftFootTarget1.up, -leftFootTarget1.up, out hit, Mathf.Infinity);
    }

    bool RightSidedRay()
    {
        // Enviar un rayo desde la posición del pie derecho hacia abajo y retornar si detecta una colisión
        return Physics.Raycast(rightFootTarget1.position + rightFootTarget1.up, -rightFootTarget1.up, out hit, Mathf.Infinity);
    }
}
