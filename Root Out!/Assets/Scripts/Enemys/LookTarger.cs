using UnityEngine;

public class LookTarger : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        LookAt();
    }
    void LookAt()
    {
       LookAtTarget(player);
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direcci�n hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,0)); // Rotaci�n de mirada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f); // Rotaci�n suave
    }
}


