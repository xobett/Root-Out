using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Start()
    {
        GetPlayerReference();
    }

    private void LateUpdate()
    {
        transform.position = target.position;
    }

    private void GetPlayerReference()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}

