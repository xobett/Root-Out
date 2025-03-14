using UnityEngine;

public class FogSpawner : MonoBehaviour
{
    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject fogPrefab;

    private const float fogDistanceSpawn = 21f;

    [Header("DETECTION SETTINGS")]
    [SerializeField, Range(1f, 5f)] private float sunflowerDetectionRadius;

    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsFog;

    private const float detectionCubeDistance = 22;

    private Vector3 detectionCube = new Vector3(5, 2, 5);
    private Vector3 detectionCubePos;

    void Start()
    {
        SetDetectionCubePos();

        if (SunflowerDetection() || GroundDetection() || FogDetection())
        {
            Destroy(gameObject);
        }
        else
        {
            SpawnFog();
        }
    }

    private void SpawnFog()
    {
        Vector3 fogSpawnPos = transform.position + transform.forward * fogDistanceSpawn;
        fogSpawnPos.y = 3.5f;

        GameObject fogClone = Instantiate(fogPrefab, fogSpawnPos, Quaternion.identity);
        fogClone.transform.parent = gameObject.transform.parent;
    }

    private void SetDetectionCubePos()
    {
        detectionCubePos = transform.position + transform.forward * detectionCubeDistance;
    }

    private bool FogDetection() => Physics.CheckBox(detectionCubePos, detectionCube / 2, Quaternion.identity, whatIsFog);
    private bool GroundDetection() => Physics.CheckBox(detectionCubePos, detectionCube / 2, Quaternion.identity, whatIsGround);
    private bool SunflowerDetection() => Physics.CheckSphere(transform.position, sunflowerDetectionRadius, whatIsSunflower);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sunflowerDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectionCubePos, detectionCube);
    }
}
