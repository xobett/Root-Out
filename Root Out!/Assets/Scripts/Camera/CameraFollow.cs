using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CAMERA FOLLOW SETTINGS")]
    [SerializeField] private Transform playerTracker; // Transform al que se emparenta la camara, con el proposito de que este siga al jugador, y la camara pueda rotar alrededor de este sin sufrir rotaciones
    //del jugador. Se enparenta la camara a este para seguirlo mientras se haga zoom.

    private GameObject player; // GameObject donde se referencia el jugador para poder rotarlo a la par de la camara.

    private Vector3 offset = new Vector3(0, 2, -8); // Vector3 donde se almacena la distancia a mantener del jugador.

    [SerializeField] private Transform followPos; // Transform en el que se almacena la posicion que seguira la camara.

    [Header("CAMERA SENSITIVITY SETTINGS")]
    [SerializeField, Range(1, 5)] private float ySensitivity; // Float donde se almacena la sensibilidad del axis Y de la camara.
    [SerializeField, Range(1, 5)] private float xSensitivity; // Float donde se almacena la sensibilidad del axis X de la camara.

    [SerializeField] private float lookUpLimit; // Float donde se almacena el limite para mirar arriba del personaje.
    [SerializeField] private float lookDownLimit; // Float donde se almacena el limite para abajo del personaje.

    [Range(1,5)] public float rotationSpeed;

    private float xRotation; // Float al que se le resta el input de rotacion Y, para restarsela a la rotacion actual.

    [Header("CAMERA ZOOM SETTINGS")]
    [SerializeField, Range(1, 10)] private float zoomSpeed; // Float donde se almacena la velocidad de zoom.
    [SerializeField] private float zoomIn; // Float donde se almacena la distancia a la que se hara zoom, misma que se usara para restarla posterior a hacer zoom.
    [SerializeField] private float zoomHorizontalOffset; // Int donde se almacena la cantidad de unidades que se movera la camara al hacer zoom.

    private Camera cam; // Camara de la cual se obtendran y modificaran los valores de zoom.

    private bool isZooming; // Bool usado para saber si se esta haciendo zoom actualmente.
    private bool aimed; // Bool para saber si se ha hecho zoom recientemente.

    void Start()
    {
        //Establece el frame rate a 60 fps.
        Application.targetFrameRate = 60;

        //Bloquea el cursor del jugador
        Cursor.lockState = CursorLockMode.Locked;

        //Establece la posicion de seguimiento de jugador. Se emparenta al Player Tracker que sigue al jugador.
        SetCameraPosition(playerTracker);

        //Referencia los componentes necesarios.
        GetReferences();
    }


    void Update()
    {
        CameraRotation();
        Aim();
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (!IsAiming() && !isZooming && !aimed)
        {
            transform.position = Vector3.Lerp(transform.position, followPos.position, 10f * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        //Checa si el jugador no esta haciendo zoom
        if (!isZooming)
        {
            //Gira la camara alrededor del Player tracker, para evitar que la camara gire junto con el jugador.
            transform.RotateAround(playerTracker.transform.position, Vector3.up, MouseHorizontalInput());
            //Gira tambien la posicion de seguimiento de la camara alrededor del Player tracker.
            followPos.transform.RotateAround(playerTracker.transform.position, Vector3.up, MouseHorizontalInput());
        }

        //Gira al jugador junto con la camara.
        player.transform.Rotate(Vector3.up * MouseHorizontalInput());


        //Se crea un Vector donde se guarda la direccion hacia el jugador.
        Vector3 relativePos = playerTracker.transform.position - transform.position;
        //Se crea un Quaternion que mirara hacia la direccion creada.
        Quaternion lookAtPlayer = Quaternion.LookRotation(relativePos, Vector3.up);

        Quaternion upRotation = Quaternion.Euler(xRotation, 0, 0);

        if (!IsAiming() && !isZooming && !aimed) 
        {
            xRotation -= MouseVerticalInput();
            xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtPlayer * upRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Aim()
    {
        if (PlayerIsGrounded())
        {
            //Para hacer zoom, checa si se esta haciendo zoom, si no se esta haciendo zoom actualmente y si no hizo zoom recientemente.
            if (IsAiming() && !aimed && !isZooming)
            {
                //Hace zoom y cambia la posicion de la camara.
                StartCoroutine(CameraZoom(zoomIn, zoomHorizontalOffset));

            }
            //Para dejar de hacer zoom, checa si ya no se esta haciendo zoom, si no se esta haciendo zoom actualmente y si se hizo zoom recientemente.
            else if (!IsAiming() && aimed && !isZooming)
            {
                //Deja de hacer zoom y regresa la posicion de la camara.
                StartCoroutine(CameraZoom(-zoomIn, -zoomHorizontalOffset));

                aimed = false;
            } 
        }
    }

    private void SetCameraPosition(Transform targetPosition)
    {
        //Establece la posicion de seguimiento a la de el transform asignado mas un offset.
        transform.position = targetPosition.position + offset;

        //Se emparenta al transform asignado para seguirlo.
        transform.parent = playerTracker.transform;
    }

    private IEnumerator CameraZoom(float zoomValue, float offsetValue)
    {
        isZooming = true;
        aimed = true;

        //Establece el valor actual del zoom de la camara.
        float currentZoom = cam.fieldOfView;
        //Establece el valor meta al que se desea cambiar el zoom de la camara.
        float targetZoom = cam.fieldOfView - zoomValue;

        //Crea un Vector donde se establece la posicion deseada a moverse cuando se haga zoom.
        Vector3 targetPos = transform.position - transform.right * offsetValue;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(4, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);

        float time = 0;

        while (time < 1)
        {
            //Cambia la rotacion del jugador progresivamente para mejor vision.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, time);
            //Cambia el zoom de la camara progresivamente.
            cam.fieldOfView = Mathf.Lerp(currentZoom, targetZoom, time);
            //Cambia la posicion del jugador progresivamente para mejor vision.
            transform.position = Vector3.Lerp(transform.position, targetPos, time);

            time += Time.deltaTime * zoomSpeed;

            yield return null;
        }

        //Para asegurar que se haya llegado a los valores deseados, se establecen al final.
        transform.position = targetPos;
        cam.fieldOfView = targetZoom;

        //La rotacion actual de la camara se resetea.
        xRotation = -10;

        isZooming = false;

        yield return null;
        
    }

    private void GetReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = gameObject.GetComponent<Camera>();
    }

    private bool PlayerIsGrounded()
    {
        return player.GetComponent<PlayerMovement>().IsTouching();
    }

    #region Inputs
    private float MouseHorizontalInput()
    {
        return Input.GetAxis("Mouse X") * xSensitivity;
    }

    private bool IsAiming()
    {
        return Input.GetMouseButton(1);
    }

    private float MouseVerticalInput()
    {
        return Input.GetAxis("Mouse Y") * ySensitivity;
    }

    #endregion

}
