using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5; // Velocidad de caminar del jugador
    [SerializeField] float runSpeed = 7; // Velocidad de correr del jugador
    [SerializeField] float jumpForce = 5f; // Fuerza del salto del jugador
    [SerializeField] float groundCheckRange = 1f; // Distancia del Raycast para verificar el suelo
    [SerializeField] LayerMask groundMask; // Máscara para detectar el suelo

    private Rigidbody rb; // Referencia al componente Rigidbody

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el componente Rigidbody del objeto
    }
    void FixedUpdate()
    {
        Move(); // Llama al método de movimiento en cada actualización de física
        CheckGround(); // Verifica si el jugador está en el suelo
    }
    void Update()
    {
        Jump(); // Llama al método de salto en cada frame
    }

    public void Move()
    {
        rb.linearVelocity = transform.rotation * new Vector3(HorizontalMove() * ActualSpeed(), rb.linearVelocity.y, VerticalMove() * ActualSpeed());
       // AudioManagerSFX.Instance.PlaySFX("Caminata"); // Reproduce el sonido de caminar
    }
    void Jump()
    {
        // Verifica si el jugador está en el suelo y presiona la tecla de salto
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica una fuerza de impulso para saltar
        }
    }

    float ActualSpeed()
    {
        return IsRunning() ? runSpeed : walkSpeed; // Operador ternario
    }
    float HorizontalMove()
    {
        return Input.GetAxis("Horizontal"); // Obtiene el valor del eje horizontal de la entrada del jugador
    }
    float VerticalMove()
    {
        return Input.GetAxis("Vertical"); // Obtiene el valor del eje vertical de la entrada del jugador
    }
    bool CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRange, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, groundCheckRange, groundMask); // Utiliza un Raycast para verificar si el jugador está en el suelo
    }
    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift); // Verifica si se está presionando la tecla de correr
    }
    public bool IsMoving()
    {
        return HorizontalMove() != 0 || VerticalMove() != 0;
    }
}
