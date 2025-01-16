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
        Vector3 move = transform.right * HorizontalInput() + transform.forward * ForwardInput(); 
        charController.Move(Time.deltaTime * SpeedCheck() * move);
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
