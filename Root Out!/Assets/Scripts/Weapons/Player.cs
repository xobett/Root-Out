using UnityEngine;


public class Player : MonoBehaviour
{
    public float walkSpeed = 5;
    public float runSpeed = 7;
    public float extraGravity = 20f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
    }

    private void Movement()
    {
        Vector3 movement = ActualSpeed() * Time.fixedDeltaTime * new Vector3(HorizontalMove(), 0, VerticalMove());
        rb.MovePosition(rb.position + transform.rotation * movement);
    }

    private float ActualSpeed()
    {
        return IsRunning() ? runSpeed : walkSpeed; // Operador ternario
    }

    public float HorizontalMove()
    {
        return Input.GetAxis("Horizontal");
    }

    public float VerticalMove()
    {
        return Input.GetAxis("Vertical");
    }

    public bool IsMoving()
    {
        if (HorizontalMove() != 0 || VerticalMove() != 0)
        {
            //Debug.Log("Me muevo");
            return true;
        }
        else
        {
            //Debug.Log("No me muevo");
            return false;
        }
    }

    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}

