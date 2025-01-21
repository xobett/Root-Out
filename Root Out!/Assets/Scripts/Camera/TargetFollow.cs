using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField, Range(0, 1)] private float followSpeed;
    [SerializeField] private Vector3 velocity;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref velocity, followSpeed * Time.deltaTime);
    }
}

