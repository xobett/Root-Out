using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform player; // Objeto a rotar
    [SerializeField] private float mouseSensitivity; // Sensibilidad del mouse
    [SerializeField] private float smoothness; // Desfase
    [SerializeField] private float minAngleY; // Minima rotacion en vertical
    [SerializeField] private float maxAngleY; // Maxima rotacion en vertical

    private Vector2 camVelocity; // Velocidad de la camara
    private Vector2 smoothVelocity; // Velocidad de los "ojos"

    [Header("Camera Movement")]
    [SerializeField] private bool moveHead; // Si está activo si sucede el movimiento de la cabeza

    [SerializeField] private float walkingSpeed; // La velocidad de la cabeza al caminar
    [SerializeField] private float runningSpeed; // La velocidad de la cabeza al correr

    [SerializeField] private float amplitude; // Que tanto se mueve
    [SerializeField] private float frequency; // Con que frecuencia se mueve
    [SerializeField] private float resetPosSpeed; // Cuanto tarda en regresar a su posicion cuando dejas de moverte

    private Vector3 startPos; // Almacena la posicion original del jugador

    public Player movementController; // Necesitamos esta referencia para saber si el personaje se está moviendo o no

    private void Start()
    {
      

        // Inicializar startPos con la posición inicial del jugador
        startPos = transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();

        if (!moveHead) return;
        BlobMove();
        ResetPosition();
    }

    #region Camera Rotation

    private void RotateCamera()
    {
        Vector2 rawFrameVelocity = Vector2.Scale(MousePos(), Vector2.one * mouseSensitivity); // Consigue hacia donde muevo el mouse y lo multiplica por la sensibilidad
        smoothVelocity = Vector2.Lerp(smoothVelocity, rawFrameVelocity, 1 / smoothness); // Mueve de (0,0) a (10,16) en 1/smoothness tiempo
        camVelocity += smoothVelocity; // camVelocity es el vector final donde almaceno hacia donde voy a voltear
        camVelocity.y = Mathf.Clamp(camVelocity.y, minAngleY, maxAngleY); // Le digo a camVelocity que solo puedo mirar hacia arriba y abajo hasta x angulo

        transform.localRotation = Quaternion.AngleAxis(-camVelocity.y, Vector3.right); // Roto la cámara hacia arriba y abajo
        player.localRotation = Quaternion.AngleAxis(camVelocity.x, Vector3.up); // Roto la capsula o player hacia los lados
    }

    private Vector2 MousePos()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    #endregion

    #region Camera Movement

    private void BlobMove()
    {
        if (!movementController.IsMoving())
        {
            return;
        }

        if (movementController.IsMoving() && !movementController.IsRunning())
        {
            Vector3 motion = FootStepMotion();
            transform.localPosition += motion;
        }
        else if (movementController.IsMoving() && movementController.IsRunning())
        {
            Vector3 motion = RunningFootStepMotion();
            transform.localPosition += motion;
        }
    }

    private void ResetPosition()
    {
        if (transform.localPosition == startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, resetPosSpeed * Time.deltaTime);
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * frequency) * amplitude * walkingSpeed;
        pos.x = Mathf.Cos(Time.time * frequency / 2) * amplitude * 2 * walkingSpeed;
        return pos;
    }

    private Vector3 RunningFootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * frequency) * amplitude * runningSpeed;
        pos.x = Mathf.Cos(Time.time * frequency / 2) * amplitude * 2 * runningSpeed;
        return pos;
    }

    #endregion
}

