using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    private GameObject target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        transform.position = target.transform.position;
    }
}

