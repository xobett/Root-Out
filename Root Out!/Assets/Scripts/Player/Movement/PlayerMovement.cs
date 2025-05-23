using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float sprintSpeed;

    private CharacterController charController;

    [Header("JUMP SETTINGS")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float gravityForce;

    private Vector2 gravityVelocity;

    public float turnSmoothVelocity;

    public Transform camRef;

    [Range(0, 1)]public float turnSpeed;


    void Start()
    {
        //Se consigue el componente de Character Controller.
        charController = GetComponent<CharacterController>();

        //Se cpmsogie el componente transform de la camara principal.
        camRef = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        MovementCheck();
    }

    private void MovementCheck()
    {
        if (IsAiming())
        {
            FaceForward();
            ZoomMovement();
        }
        else
        {
            NormalMovement();
        }
    }

    private void ZoomMovement()
    {
        Vector3 zoomMovement = transform.right * HorizontalInput() + transform.forward * ForwardInput();

        charController.Move(Time.deltaTime * SpeedCheck() * zoomMovement);
    }

    private void NormalMovement()
    {
        Vector3 move = new Vector3(HorizontalInput(), 0, ForwardInput());

        if (move.magnitude != 0f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + camRef.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = transform.rotation * Vector3.forward;

            charController.Move(Time.deltaTime * SpeedCheck() * moveDirection);
        }
    }

    private void FaceForward()
    {
        //Se crea un angulo que interpola suavemente la rotacion en Y del jugador con la de la camara.
        float faceForwardAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camRef.eulerAngles.y, ref turnSmoothVelocity, turnSpeed);
        //Se asigna el angulo creado a la rotacion.
        transform.rotation = Quaternion.Euler(0, faceForwardAngle, 0f);
    }

    private void Gravity()
    {
        
    }

    private float SpeedCheck()
    {
        //Regresa velocidad de sprint si esta haciendo sprint el jugador, de lo contrario regresa velocidad normal.
        return IsSprinting() ? sprintSpeed : walkSpeed;
    }


    private float HorizontalInput()
    {
        //Regresa Input en X (Hacia la derecha e izquierda del jugador)
        return Input.GetAxis("Horizontal");
    }

    private float ForwardInput()
    {
        //Regresa Input en Z (Hacia el frente y atras del jugador)
        return Input.GetAxis("Vertical");
    }
    private bool IsSprinting()
    {
        //Regresa si el jugador esta sprintando
        return Input.GetKey(KeyCode.LeftShift);
    }

    private bool IsAiming()
    {
        //Regresa si el jugador esta apuntando
        return Input.GetMouseButton(1);
    }
}
