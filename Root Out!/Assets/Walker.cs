using UnityEngine;


public class Walker : MonoBehaviour
{
    [SerializeField] AnimationCurve horizontalCurve; // Curva de animaci�n para el movimiento horizontal del pie
    [SerializeField] AnimationCurve verticalCurve; // Curva de animaci�n para el movimiento vertical del pie
    [SerializeField] Transform rightFootTarget1; // Transform del objetivo del pie derecho
    [SerializeField] Transform leftFootTarget1; // Transform del objetivo del pie izquierdo

    Vector3 rigthOffsetTarget1; // Desplazamiento inicial del pie derecho
    Vector3 leftOffsetTarget1; // Desplazamiento inicial del pie izquierdo

    float rightLegForwadMovement; // Movimiento hacia adelante del pie derecho
    float leftLegForwadMovement; // Movimiento hacia adelante del pie izquierdo

    float rightLegLast; // �ltima posici�n hacia adelante del pie derecho
    float leftLegLast; // �ltima posici�n hacia adelante del pie izquierdo

    RaycastHit hit; // Informaci�n del impacto del rayo

    Vector3 lastPosition; // �ltima posici�n del personaje

    float timeOffset = 0.5f; // Desplazamiento de tiempo para desfasar los pasos

    private void Start()
    {
        // Inicializar el desplazamiento del pie derecho e izquierdo a sus posiciones locales actuales
        rigthOffsetTarget1 = rightFootTarget1.localPosition;
        leftOffsetTarget1 = leftFootTarget1.localPosition;

        // Inicializar la �ltima posici�n del personaje a su posici�n actual
        lastPosition = transform.position;
    }

    private void Update()
    {
        // Si el personaje se est� moviendo, realizar la caminata procedural
        if (IsMoving())
        {
            ProceduralWalk();
        }

        // Actualizar la �ltima posici�n del personaje a su posici�n actual
        lastPosition = transform.position;
    }

    void ProceduralWalk()
    {
        // Ejecutar los movimientos de las piernas y la detecci�n de colisiones
        LeftWalk();
        RightWalk();
        LeftDetection();
        RightDetection();
    }

    void RightWalk()
    {
        // Evaluar la curva de animaci�n para el pie derecho con un desplazamiento de tiempo
        float curveTime = (Time.time + timeOffset) % 1f;
        rightLegForwadMovement = horizontalCurve.Evaluate(curveTime);

        // Actualizar la posici�n local del pie derecho basado en las curvas de animaci�n horizontal y vertical
        rightFootTarget1.localPosition = rigthOffsetTarget1 +
            this.transform.forward * rightLegForwadMovement + // Corregir direcci�n hacia adelante
            this.transform.up * verticalCurve.Evaluate(curveTime);

        // Actualizar la �ltima posici�n hacia adelante del pie derecho
        rightLegLast = rightLegForwadMovement;
    }

    void LeftWalk()
    {
        // Evaluar la curva de animaci�n para el pie izquierdo sin desplazamiento de tiempo
        float curveTime = Time.time % 1f;
        leftLegForwadMovement = horizontalCurve.Evaluate(curveTime);

        // Actualizar la posici�n local del pie izquierdo basado en las curvas de animaci�n horizontal y vertical
        leftFootTarget1.localPosition = leftOffsetTarget1 +
            this.transform.forward * leftLegForwadMovement + // Corregir direcci�n hacia adelante
            this.transform.up * verticalCurve.Evaluate(curveTime);

        // Actualizar la �ltima posici�n hacia adelante del pie izquierdo
        leftLegLast = leftLegForwadMovement;
    }

    void RightDetection()
    {
        // Calcular la direcci�n del movimiento del pie derecho
        float rightLegDirectional = rightLegForwadMovement - rightLegLast;

        // Si el pie derecho se est� moviendo hacia adelante y detecta una colisi�n, actualizar la posici�n del pie derecho
        if (rightLegDirectional > 0 && RightSidedRay())
        {
            rightFootTarget1.position = hit.point;
        }
    }

    void LeftDetection()
    {
        // Calcular la direcci�n del movimiento del pie izquierdo
        float leftLegDirectional = leftLegForwadMovement - leftLegLast;

        // Si el pie izquierdo se est� moviendo hacia atr�s y detecta una colisi�n, actualizar la posici�n del pie izquierdo
        if (leftLegDirectional < 0 && LeftSidedRay())
        {
            leftFootTarget1.position = hit.point;
        }
    }

    bool IsMoving()
    {
        // Comparar la posici�n actual con la �ltima posici�n para determinar si el personaje se est� moviendo
        return Vector3.Distance(transform.position, lastPosition) > 0.01f; // Ajustar el umbral seg�n sea necesario
    }

    bool LeftSidedRay()
    {
        // Enviar un rayo desde la posici�n del pie izquierdo hacia abajo y retornar si detecta una colisi�n
        return Physics.Raycast(leftFootTarget1.position + leftFootTarget1.up, -leftFootTarget1.up, out hit, Mathf.Infinity);
    }

    bool RightSidedRay()
    {
        // Enviar un rayo desde la posici�n del pie derecho hacia abajo y retornar si detecta una colisi�n
        return Physics.Raycast(rightFootTarget1.position + rightFootTarget1.up, -rightFootTarget1.up, out hit, Mathf.Infinity);
    }
}
