using UnityEngine;

public class LookTarger : MonoBehaviour
{
    private Transform cameraPlayer;

    private void Start()
    {
        cameraPlayer = FindAnyObjectByType<Camera>().transform;
    }
    private void LateUpdate()
    {
        LookAtTarget(cameraPlayer);
    }
    void LookAtTarget(Transform target) // M�todo para mirar al Target
    {
        Vector3 direction = (target.position - transform.position).normalized; // Direcci�n hacia el target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotaci�n de mirada solo horizontal
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f); // Rotaci�n suave
    }
}


