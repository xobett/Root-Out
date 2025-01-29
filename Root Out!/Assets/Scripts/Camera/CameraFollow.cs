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

    private float xRotation;

    public bool testBool;

    public Vector3 targetPos;

    void Start()
    {
        SetCameraPosition();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraRotation();
        ResetCameraPosition();
    }

    private void CameraRotation()
    {
        xRotation -= MouseVerticalInput();
        xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);

        Quaternion upRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.RotateAround(followTarget.transform.position, Vector3.up, MouseHorizontalInput());

        Vector3 relativePos = followTarget.transform.position - transform.position;

        Quaternion lookAtPlayer = Quaternion.LookRotation(relativePos, Vector3.up);

        transform.rotation = lookAtPlayer * upRotation;
    }

    private void SetCameraPosition()
    {
        transform.position = followTarget.transform.position + offset;

        transform.parent = followTarget.transform;
    }

    private void ResetCameraPosition()
    {
        targetPos = followTarget.transform.position + offset;

        if (testBool)
        {
            transform.position = Vector3.Slerp(transform.position, targetPos, 0.008f);
        }
    }

    private float MouseHorizontalInput()
    {
        return Input.GetAxis("Mouse X") * xSensitivity;
    }

    private float MouseVerticalInput()
    {
        return Input.GetAxis("Mouse Y") * ySensitivity;
    }
}
