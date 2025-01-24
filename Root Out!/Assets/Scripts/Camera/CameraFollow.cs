using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CAMERA FOLLOW SETTINGS")]
    [SerializeField, Range(0, 1)] private float followSpeed;

    [SerializeField] private Vector3 offset;
    private Vector3 velocity =  Vector3.zero;

    private GameObject player;

    [Header("CAMERA SENSITIVITY SETTINGS")]

    [SerializeField, Range(1, 5)] private float ySensitivity;
    [SerializeField, Range(1, 500)] private float xSensitivity;

    [SerializeField] private float lookUpLimit;
    [SerializeField] private float lookDownLimit;

    private float xRotation;

    private Vector3 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = transform.position + offset;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //CameraRotation();
        PlayerFollow();
    }

    private void CameraRotation()
    {
        xRotation -= MouseVerticalInput();
        xRotation = Mathf.Clamp(xRotation, lookDownLimit, lookUpLimit);

        //transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


    }

    private void LateUpdate()
    {
        PlayerFollow();
    }

    private void PlayerFollow()
    {

        Quaternion targetRotation = Quaternion.Euler(MouseVerticalInput(), MouseHorizontalInput(), 0);

        target = player.transform.position + offset;

        transform.position = targetRotation * target;
        //transform.rotation = targetRotation;
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
