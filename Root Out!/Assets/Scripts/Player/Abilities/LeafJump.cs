
using UnityEngine;

public class LeafJump : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField, Range(5, 20)] private float totalChargeLimit = 15f;
    public float currentChargedForce;

    [Header("ANIMATION SETTINGS")]
    [SerializeField] private Animator playerAnimCtrlr;

    void Update()
    {
        if (!GameManager.instance.gamePaused)
        {
            ChargeJump();
            Jump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        if (collision.gameObject.layer == groundLayer)
        {
            AudioManagerSFX.Instance.PlaySFX("Aterrizaje");
        } 
    }

    private void Jump()
    {
        //Checa si se dejo de presionar la barra espaciadora y si el jugador se encuentra en el suelo.
        if (IsJumping() && PlayerIsGrounded() && currentChargedForce > 5)
        {
            //Se agrega fuerza positiva sobre el Vector3 que constantemente ejerce gravedad.
            GetComponent<PlayerMovement>().gravity.y = currentChargedForce;

            playerAnimCtrlr.SetBool("isJumping", true);

            AudioManagerSFX.Instance.PlaySFX("Salto");

            //Tras saltar, se reinicia el valor del salto cargado.
            currentChargedForce = 0f;
        }
        else
        {
            playerAnimCtrlr.SetBool("isJumping", false);
        }
    }

    private void ChargeJump()
    {
        //Checa si se esta presionando la barra espaciadora, si el jugador esta en el suelo, y si la carga del salto no esta cargada al maximo.
        if (Input.GetKey(KeyCode.Space) && PlayerIsGrounded() && currentChargedForce < totalChargeLimit)
        {
            //Añade fuerza al salto cada frame.
            currentChargedForce += 0.25f;
        }
    }

    private bool PlayerIsGrounded()
    {
        return gameObject.GetComponent<PlayerMovement>().IsTouching();
    }

    private bool IsJumping() => Input.GetKeyUp(KeyCode.Space);
}
