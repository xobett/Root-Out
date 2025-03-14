
using UnityEngine;

public class LeafJump : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField, Range(5, 20)] private float totalChargeLimit = 15f;
    private float currentChargedForce;

    void Update()
    {
        ChargeJump();
        Jump();
    }

    private void Jump()
    {
        //Checa si se dejo de presionar la barra espaciadora y si el jugador se encuentra en el suelo.
        if (Input.GetKeyUp(KeyCode.Space) && PlayerIsGrounded())
        {
            //Se agrega fuerza positiva sobre el Vector3 que constantemente ejerce gravedad.
            GetComponent<PlayerMovement>().gravity.y = currentChargedForce;

            //Tras saltar, se reinicia el valor del salto cargado.
            currentChargedForce = 0f;
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
}
