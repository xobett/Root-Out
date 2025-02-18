using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerMovement : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float sprintSpeed;

    [SerializeField] private float turnSmoothVelocity;
    [Range(0, 1)] public float turnSpeed;

    private CharacterController charController;

    [Header("JUMP SETTINGS")]
    [SerializeField] private float chargedJumpForce;

    [Header("GRAVITY SETTINGS")]
    [SerializeField] private float gravityForce;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform groundCheck;

    [SerializeField, Range(0f, 1f)] private float groundCheckRadius;

    [SerializeField] public Vector2 gravity;

    private Transform camRef;

    void Start()
    {
        //Se consigue el componente de Character Controller.
        charController = GetComponent<CharacterController>();

        //Se consigue el componente transform de la camara principal.
        camRef = Camera.main.transform;
    }

    void Update()
    {
        MovementCheck();
        Gravity();
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
        //Mueve al jugador de izquierda a derecha, de frente hacia atras sin rotar a la direccion a la que camina.
        //Se usa al apuntar para fijar la mira del personaje al disparar.
        Vector3 zoomMovement = transform.right * HorizontalInput() + transform.forward * ForwardInput();

        //Mueve al personaje sin rotacion.
        charController.Move(Time.deltaTime * SpeedCheck() * zoomMovement);
    }

    private void NormalMovement()
    {
        //Se crea un Vector3 donde se almacenara en X el input de izquierda a derecha del jugador, y en Z el input de frente hacia atras del jugador.
        //Se crea de esta manera para que despues se pueda sacar un angulo entre el axis X y Z.
        Vector3 move = new Vector3(HorizontalInput(), 0, ForwardInput());

        //Como constantemente se avanza hacia la rotacion del jugador, solamente se mueve el personaje si hay un input desigual a 0.
        if (move.magnitude != 0f)
        {
            //Obtiene un angulo en radianes creado entre el axis X y Z, lo convierte en grados y suma la rotacion frontal de la camara.
            //El angulo total que se obtiene es para rotar al jugador hacia la direccion donde camina y hacia la camara.
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + camRef.eulerAngles.y;
            //Interpola suavemente la rotacion actual del jugador a el angulo creado.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            //Aplica la rotacion hacia el angulo deseado.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Se crea un Vector3 donde se multiplica la rotacion del jugador hacia delante.
            Vector3 moveDirection = transform.rotation * Vector3.forward;

            //Mueve al jugador hacia donde este rotado.
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
        //Le resta al Vector3 en su eje Y la cantidad de gravedad determinada.
        gravity.y -= gravityForce * Time.deltaTime;

        //Checa si esta tocando el suelo y la velocidad ejercida en Y es menor a 0.
        if (IsTouching() && gravity.y < 0)
        {
            //Si el condicional es cierto, se deja de restar valor al Vector3 en su axis Y.
            gravity.y = 0;
        }

        //Aplica constantemente la fuerza de gravedad.
        charController.Move(gravity * Time.deltaTime);
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

    public bool IsTouching()
    {
        bool isTouching = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
        return isTouching;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
