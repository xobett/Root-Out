using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Start()
    {
      
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + target.position);
    }
}

