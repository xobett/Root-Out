using UnityEngine;

public class FogSpawner : MonoBehaviour
{
    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject fogPrefab;

    [Header("DETECTION SETTINGS")]
    [SerializeField, Range(1f, 5f)] private float sunflowerDetectionRadius;
    [SerializeField] private LayerMask whatIsSunflower;


    void Start()
    {
        if (SunflowerDetection()) Destroy(gameObject);
    }

    private bool SunflowerDetection() => Physics.CheckSphere(transform.position, sunflowerDetectionRadius, whatIsSunflower);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sunflowerDetectionRadius);
    }
}
