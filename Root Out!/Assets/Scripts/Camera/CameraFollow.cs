using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CAMERA FOLLOW SETTINGS")]
    [SerializeField, Range(0, 1)] private float followSpeed;

    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform followTarget;

    [Header("CAMERA SENSITIVITY SETTINGS")]

    [SerializeField, Range(1, 5)] private float ySensitivity;
    [SerializeField, Range(1, 500)] private float xSensitivity;

    [SerializeField] private float lookUpLimit;
    [SerializeField] private float lookDownLimit;

    [Header("CAMERA ZOOM SETTINGS")]
    [SerializeField, Range(1, 10)] private float zoomSpeed;
    [SerializeField] private float zoomIn;
    [SerializeField] private float zoomHorizontalOffset;

    private bool zooming;
    private bool postAimed;

    private float xRotation;

    public bool testBool;

    private GameObject player;

     private Camera cam;

    void Start()
    {
        SetCameraPosition(followTarget);

        Cursor.lockState = CursorLockMode.Locked;

        player = GameObject.FindGameObjectWithTag("Player");

        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        CameraRotation();
        Aim();
    }

    private void CameraRotation()
    {
        xRotation -= MouseVerticalInput();
        xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);

        Quaternion upRotation = Quaternion.Euler(xRotation, 0, 0);

        player.transform.Rotate(Vector3.up * MouseHorizontalInput());

        transform.RotateAround(followTarget.transform.position, Vector3.up, MouseHorizontalInput());

        Vector3 relativePos = followTarget.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativePos, Vector3.up);

        if (!IsAiming())
        {
            transform.rotation = lookAtPlayer * upRotation; 
        }
    }

    private void Aim()
    {

        if (IsAiming() && !postAimed && !zooming)
        {
            StartCoroutine(CameraZoom(zoomIn, zoomHorizontalOffset));

            //Tiene que hacer zoom
        }
        else if (!IsAiming() && postAimed && !zooming)
        {
            StartCoroutine(CameraZoom(-zoomIn, -zoomHorizontalOffset));

            postAimed = false;
        }
    }

    private void SetCameraPosition(Transform targetPosition)
    {
        transform.position = targetPosition.position + offset;

        transform.parent = followTarget.transform;
    }

    private float MouseHorizontalInput()
    {
        return Input.GetAxis("Mouse X") * xSensitivity;
    }

    private float MouseVerticalInput()
    {
        return Input.GetAxis("Mouse Y") * ySensitivity;
    }

    private bool IsAiming()
    {
        return Input.GetMouseButton(1);
    }

    private IEnumerator CameraZoom(float zoomValue, float offsetValue)
    {
        zooming = true;
        postAimed = true;

        float currentZoom = cam.fieldOfView;
        float targetZoom = cam.fieldOfView - zoomValue;

        Vector3 targetPos = transform.position + transform.right * offsetValue;

        float time = 0;

        while (time < 1)
        {
            cam.fieldOfView = Mathf.Lerp(currentZoom, targetZoom, time);
            transform.position = Vector3.Lerp(transform.position, targetPos, time);
            time += Time.deltaTime * zoomSpeed;

            yield return null;
        }

        cam.fieldOfView = targetZoom;

        zooming = false;

        yield return null;
        
    }
}
