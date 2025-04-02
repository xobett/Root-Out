using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("NEW CAMERA FOLLOW SETTINGS")]
    [SerializeField] private Transform followTarget; //Transform al que seguira la camara.

    [SerializeField] private float maxDistance; //Float que controla la distancia de la orbita alrededor del jugador.

    [SerializeField] private Vector2 cameraSensitivity; //Vector2 que controla la sensibilidad de la camara.

    Vector2 orbitAngle = new Vector2(-90 * Mathf.Deg2Rad, 0); //Vector2 que recibe Input del mouse convertido a radianes, para crear un orbita en base a este con las funciones COS y SIN.

    [SerializeField] private float upLimit; // Float que sera convertido a radianes para limitar la vista hacia arriba de la camara.
    [SerializeField] private float downLimit; // Float que sera convertido a radianes para limitar la vista hacia abajo de la camara.

    private GameObject player; // GameObject donde se referencia el jugador para poder rotarlo a la par de la camara.

    private const float orbitSmooth = 15f; //Float que controla la suavidad de la orbita alrededor del jugador.

    [Header("CAMERA ZOOM SETTINGS")]
    [SerializeField, Range(1, 10)] private float zoomSpeed; // Float donde se almacena la velocidad de zoom.
    [SerializeField] private float zoomIn; // Float donde se almacena la distancia a la que se hara zoom, misma que se usara para restarla posterior a hacer zoom.
    [SerializeField] private float horizontalZoomOffset;

    public bool isZooming; // Bool usado para saber si se esta haciendo zoom actualmente.
    private bool aimed; // Bool para saber si se ha hecho zoom recientemente.

    private Camera cam; // Camara de la cual se obtendran y modificaran los valores de zoom.

    [Header("CAMERA COLLISION SETTINGS")]
    [SerializeField] private LayerMask whatIsCollision; //Layer que controla en que layers se detecta colision.
    [SerializeField] private Vector2 nearPlaneSize;
    [SerializeField, Range(0f, 1f)] private float safeDistance;
    private RaycastHit hit;

    void Start()
    {
        //Establece el frame rate a 60 fps.
        Application.targetFrameRate = 60;

        //Bloquea el cursor del jugador
        Cursor.lockState = CursorLockMode.Locked;

        //Referencia los componentes necesarios.
        GetReferences();

        GetCameraNearPlaneSize();
    }

    void Update()
    {
        Aim();
        OrbitRotationInput();
    }

    private void LateUpdate()
    {
        FollowAndOrbit();
    }

    private void GetCameraNearPlaneSize()
    {
        float planeHeight = Mathf.Tan((cam.fieldOfView * Mathf.Deg2Rad / 2) * cam.nearClipPlane);
        float planeWidth = planeHeight * cam.aspect;

        nearPlaneSize = new Vector2(planeWidth, planeHeight);
    }

    private Vector3[] CalculateCollisionPoints(Vector3 orbitDirection)
    {
        Vector3 position = followTarget.position;
        Vector3 center = position + orbitDirection * (cam.nearClipPlane + safeDistance);

        Vector3 right = transform.right * nearPlaneSize.x;
        Vector3 up = transform.up * nearPlaneSize.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }

    private void OrbitRotationInput()
    {
        if (MouseHorizontalInput() != 0)
        {
            orbitAngle.x -= MouseHorizontalInput() * Mathf.Deg2Rad * cameraSensitivity.x * Time.deltaTime;
        }

        if (MouseVerticalInput() != 0)
        {
            orbitAngle.y -= MouseVerticalInput() * Mathf.Deg2Rad * cameraSensitivity.y * Time.deltaTime;

            orbitAngle.y = Mathf.Clamp(orbitAngle.y, downLimit * Mathf.Deg2Rad, upLimit * Mathf.Deg2Rad);
        }
    }
    private void FollowAndOrbit()
    {
        //Se crea un Vector3 donde se almacenara la rotacion activa alrededor del objetivo.
        Vector3 orbitValue;

        //En un circulo, para calcular la posicion alrededor de este se usa la funcion de Coseno para calcular posicion en X, y la funcion Seno para calcular la posicion en Y.
        //Dicho eso, el vector crea por asi decirlo un circulo alrededor del angulo que recibe en radianes, calculando la posicion X con el COS y la posicion Y con SIN.
        Vector3 normalOrbit = new Vector3(Mathf.Cos(orbitAngle.x) * Mathf.Cos(orbitAngle.y), Mathf.Sin(orbitAngle.y), Mathf.Sin(orbitAngle.x) * Mathf.Cos(orbitAngle.y));

        //Se crea un Vector3 donde se crea una nueva rotacion en orbita solamente en los ejes X y Z, para evitar rotar hacia arriba al hacer zoom.
        //NO USADO PERO SE GUARDA POR SI ACASO.
        Vector3 zoomOrbit = new Vector3(Mathf.Cos(orbitAngle.x), 0, Mathf.Sin(orbitAngle.x));

        orbitValue = normalOrbit;

        float orbitDistance = maxDistance;
        Vector3[] collisionPoints = CalculateCollisionPoints(orbitValue);

        foreach (Vector3 collisionPoint in collisionPoints)
        {
            if (Physics.Raycast(collisionPoint, orbitValue, out hit, maxDistance, whatIsCollision))
            {
                orbitDistance = Mathf.Min((hit.point - followTarget.position).magnitude, orbitDistance);
            }
        }

        //La posicion de la camara sigue la posicion del jugador, sumandole los valores del Vector3 que controla la rotacion orbita creada, multiplicando la distancia de la orbita por un float.
        Vector3 orbitMovement = followTarget.position + orbitValue * orbitDistance;
        //Se interpola esfericamente la posicion de la camara.
        transform.position = Vector3.Slerp(transform.position, orbitMovement, orbitSmooth * Time.deltaTime);

        Quaternion lookAtPlayer = Quaternion.LookRotation(followTarget.position - transform.position, Vector3.up);
        //Mira constantemente al jugador.
        transform.rotation = lookAtPlayer;

        //Gira al jugador en conjunto con la rotacion de la camara.
        player.transform.Rotate(Vector3.up * MouseHorizontalInput() * cameraSensitivity.x * Time.deltaTime);
    }

    private void Aim()
    {
        if (PlayerIsGrounded())
        {
            //Para hacer zoom, checa si se esta haciendo zoom, si no se esta haciendo zoom actualmente y si no hizo zoom recientemente.
            if (IsAiming() && !aimed && !isZooming)
            {
                //Hace zoom y cambia la posicion de la camara.
                StartCoroutine(CameraZoom(zoomIn, horizontalZoomOffset));

            }
            //Para dejar de hacer zoom, checa si ya no se esta haciendo zoom, si no se esta haciendo zoom actualmente y si se hizo zoom recientemente.
            else if (!IsAiming() && aimed && !isZooming)
            {
                //Deja de hacer zoom y regresa la posicion de la camara.
                StartCoroutine(CameraZoom(-zoomIn, -horizontalZoomOffset));

                aimed = false;
            }
        }
    }

    private IEnumerator CameraZoom(float zoomValue, float horizontalOffset)
    {
        isZooming = true;
        aimed = true;

        //Establece el valor meta al que se desea cambiar el zoom de la camara.
        float targetZoom = cam.fieldOfView - zoomValue;

        //Dado a que la posicion de seguimiento es hijo del jugador, se asigna en un Vector la posicion local de este en relacion al padre.
        Vector3 targetPosition = followTarget.localPosition;
        //Despues, a el valor en X de esa posicion en horizontal se le asigna el valor a aumentar en X en su posicion local.
        targetPosition.x += horizontalOffset;

        float time = 0;

        while (time < 1)
        {
            //Cambia el zoom de la camara progresivamente.
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, time);
            //Cambia la rotacion en Y de la camara a 0 para mejor vista.
            orbitAngle.y = Mathf.Lerp(orbitAngle.y, 10 * Mathf.Deg2Rad, time);
            //Cambia la posicion de la camara para mejor vision.
            followTarget.localPosition = Vector3.Lerp(followTarget.localPosition, targetPosition, time);

            time += Time.deltaTime * zoomSpeed;

            yield return null;
        }

        //Para asegurar que se haya llegado a los valores deseados, se establecen al final.
        cam.fieldOfView = targetZoom;
        followTarget.localPosition = targetPosition;

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
    private float MouseHorizontalInput() => Input.GetAxis("Mouse X");
    private float MouseVerticalInput() => Input.GetAxis("Mouse Y");
    private bool IsAiming() => Input.GetMouseButton(1);

    #endregion

}
