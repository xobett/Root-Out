using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CAMERA FOLLOW SETTINGS")]
    [SerializeField, Range(0, 1)] private float followSpeed;

    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform followTarget;

    private GameObject player;

    [Header("CAMERA SENSITIVITY SETTINGS")]

    [SerializeField, Range(1, 5)] private float ySensitivity;
    [SerializeField, Range(1, 500)] private float xSensitivity;

    [SerializeField] private float lookUpLimit;
    [SerializeField] private float lookDownLimit;

    private float xRotation;

    [Header("CAMERA ZOOM SETTINGS")]
    [SerializeField, Range(1, 10)] private float zoomSpeed;
    [SerializeField] private float zoomIn;
    [SerializeField] private int zoomHorizontalOffset;

    private Camera cam;

    private bool zooming;
    private bool postAimed;

    [SerializeField] private Transform followPos;

    public bool testBool;
    [Range(1,5)] public float testVelo;

    void Start()
    {
        Application.targetFrameRate = 60;

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

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (!IsAiming() && !zooming && !postAimed)
        {
            transform.position = Vector3.Lerp(transform.position, followPos.position, 4f * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        xRotation -= MouseVerticalInput();
        xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);

        Quaternion upRotation = Quaternion.Euler(xRotation, 0, 0);

        player.transform.Rotate(Vector3.up * MouseHorizontalInput());

        if (!zooming)
        {
            transform.RotateAround(followTarget.transform.position, Vector3.up, MouseHorizontalInput());
            followPos.transform.RotateAround(followTarget.transform.position, Vector3.up, MouseHorizontalInput());
        }

        Vector3 relativePos = followTarget.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativePos, Vector3.up);

        if (!IsAiming() && !zooming && !postAimed) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtPlayer * upRotation, testVelo * Time.deltaTime);
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

            xRotation = 0;
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

    private IEnumerator CameraZoom(float zoomValue, int offsetValue)
    {
        zooming = true;
        postAimed = true;

        float currentZoom = cam.fieldOfView;
        float targetZoom = cam.fieldOfView - zoomValue;

        Vector3 targetPos = transform.position - transform.right * offsetValue;

        Debug.Log(targetPos);

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

        zooming = false;

        yield return null;
        
    }
}
