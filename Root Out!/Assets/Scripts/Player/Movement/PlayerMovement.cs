using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    private CharacterController charController;

    [Header("JUMP SETTINGS")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float gravityForce;

    private Vector2 gravityVelocity;

    public float turnSmoothVelocity;

    [Range(0, 1)]public float turnSpeed;
    void Start()
    {
        //Se consigue el componente de Character Controller
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //Creo un Vector3, sumo el vector de la derecha del jugador multiplicado por el input, mas el vector de enfrente del jugador multiplicado por el input.
        //El Vector3 es usado por el metodo Move mientras checa constantemente la velocidad actual.
        Vector3 move = new Vector3(HorizontalInput(), 0, ForwardInput());

        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            charController.Move(Time.deltaTime * SpeedCheck() * move);
        }
    }

    private void Jump()
    {

    }

    private float SpeedCheck()
    {
        //Regresa velocidad de sprint si esta haciendo sprint el jugador, de lo contrario regresa velocidad normal.
        return IsSprinting() ? sprintSpeed : walkSpeed;
    }

    private bool IsSprinting()
    {
        //Regresa si el jugador esta sprintando
        return Input.GetKey(KeyCode.LeftShift);
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
}
