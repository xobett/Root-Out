using UnityEngine;

public class LookTarger : MonoBehaviour
{
    [SerializeField] private Transform cameraPlayer;

    private void LateUpdate()
    {
        LookAt();
    }
    void LookAt()
    {
        LookAtTarget(cameraPlayer);
    }
    void LookAtTarget(Transform target)
    {
        transform.LookAt(cameraPlayer.position);
    }
}


