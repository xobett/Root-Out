using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField, Range(0, 1)] private float followSpeed;
    private Vector3 velocity =  Vector3.zero;

    [SerializeField, Range(0, 50)] private float speed;

    private float xRotation;

    private GameObject player;

    [SerializeField] private Vector3 offset;
    private Vector3 target;

    void Start()
    {
        //
        player = GameObject.FindGameObjectWithTag("Player");
        target = transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        //FollowPlayer();
    }

    private void FollowPlayer()
    {
        target = player.transform.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, followSpeed);

        transform.RotateAround(player.transform.position, Vector3.up, speed * Time.deltaTime);

        //xRotation -= MouseVerticalInput();

        //transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Vector3 relativePos = player.transform.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);

        //Quaternion current = transform.localRotation;

        //transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
        //transform.Translate(0, 0, 3);
    }

    //private void LateUpdate()
    //{
    //    target = player.transform.position + offset;

    //    transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, followSpeed);
    //}

    private float MouseHorizontalInput()
    {
        return Input.GetAxis("Mouse X");
    }

    private float MouseVerticalInput()
    {
        return Input.GetAxis("Mouse Y");
    }
}
