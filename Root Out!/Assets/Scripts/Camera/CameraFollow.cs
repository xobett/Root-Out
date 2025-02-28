using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CAMERA FOLLOW SETTINGS")]
    [SerializeField] private Transform playerTracker; // Transform al que se emparenta la camara, con el proposito de que este siga al jugador, y la camara pueda rotar alrededor de este sin sufrir rotaciones
    //del jugador. Se enparenta la camara a este para seguirlo mientras se haga zoom.

    private GameObject player; // Se referencia el jugador para poder rotarlo a la par de la camara.

    private Vector3 offset = new Vector3(0, 2, -8); // Distancia a mantener del jugador.

    [Header("CAMERA SENSITIVITY SETTINGS")]

    [SerializeField, Range(1, 5)] private float ySensitivity; // Sensibilidad del axis Y de la camara.
    [SerializeField, Range(1, 5)] private float xSensitivity; // Sensibilidad del axis X de la camara.

    [SerializeField] private float lookUpLimit; // Limite para mirar arriba del personaje.
    [SerializeField] private float lookDownLimit; // Limite para abajo del personaje.

    private float xRotation;

    [Header("CAMERA ZOOM SETTINGS")]
    [SerializeField, Range(1, 10)] private float zoomSpeed;
    [SerializeField] private float zoomIn;
    [SerializeField] private int zoomHorizontalOffset;

    private Camera cam;

    private bool isZooming;
    private bool aimed;

    [SerializeField] private Transform followPos;

    public bool testBool;
    [Range(1,5)] public float testVelo;

    void Start()
    {
        Application.targetFrameRate = 60;

        SetCameraPosition(playerTracker);

        Cursor.lockState = CursorLockMode.Locked;

        player = GameObject.FindGameObjectWithTag("Player");

        cam = gameObject.GetComponent<Camera>();

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
            transform.position = Vector3.Lerp(transform.position, followPos.position, 4f * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        Quaternion upRotation = Quaternion.Euler(xRotation, 0, 0);

        player.transform.Rotate(Vector3.up * MouseHorizontalInput());

        if (!isZooming)
        {
            transform.RotateAround(playerTracker.transform.position, Vector3.up, MouseHorizontalInput());
            followPos.transform.RotateAround(playerTracker.transform.position, Vector3.up, MouseHorizontalInput());
        }

        Vector3 relativePos = playerTracker.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativePos, Vector3.up);

        if (!IsAiming() && !isZooming && !aimed) 
        {
            xRotation -= MouseVerticalInput();
            xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtPlayer * upRotation, testVelo * Time.deltaTime);
        }
    }

    private void Aim()
    {
        if (IsAiming() && !aimed && !isZooming)
        {
            StartCoroutine(CameraZoom(zoomIn, zoomHorizontalOffset));

        }
        else if (!IsAiming() && aimed && !isZooming)
        {
            StartCoroutine(CameraZoom(-zoomIn, -zoomHorizontalOffset));

            aimed = false;
        }
    }

    private void SetCameraPosition(Transform targetPosition)
    {
        transform.position = targetPosition.position + offset;

        transform.parent = playerTracker.transform;
    }

    private IEnumerator CameraZoom(float zoomValue, int offsetValue)
    {
        isZooming = true;
        aimed = true;

        float currentZoom = cam.fieldOfView;
        float targetZoom = cam.fieldOfView - zoomValue;

        Vector3 targetPos = transform.position - transform.right * offsetValue;

        float time = 0;

        while (time < 1)
        {
            cam.fieldOfView = Mathf.Lerp(currentZoom, targetZoom, time);
            transform.position = Vector3.Lerp(transform.position, targetPos, time);
            time += Time.deltaTime * zoomSpeed;

            yield return null;
        }

        transform.position = targetPos;
        cam.fieldOfView = targetZoom;

        xRotation = -10;

        isZooming = false;

        yield return null;
        
    }

    private bool IsAiming()
    {
        return Input.GetMouseButton(1);
    }

    #region Inputs
    private float MouseHorizontalInput()
    {
        return Input.GetAxis("Mouse X") * xSensitivity;
    }

    private float MouseVerticalInput()
    {
        return Input.GetAxis("Mouse Y") * ySensitivity;
    }

    #endregion
}
