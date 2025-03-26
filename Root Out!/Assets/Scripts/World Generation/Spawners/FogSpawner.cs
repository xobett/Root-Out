using UnityEngine;

public class FogSpawner : MonoBehaviour
{
    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject fogPrefab;

    private const float fogDistanceSpawn = 22f;

    [Header("DETECTION SETTINGS")]
    [SerializeField, Range(1f, 5f)] private float sunflowerDetectionRadius;

    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsFog;

    private const float detectionCubeDistance = 23f;

    private Vector3 detectionCube = new Vector3(6, 2, 6);
    private Vector3 detectionCubePos;

    void Start()
    {
        SetDetectionCubePos();

        if (SunflowerDetection() || GroundDetection() || FogDetection())
        {
            Destroy(this.gameObject);
        }
        else
        {
            SpawnFog();
            Debug.Log("Spawned fog");
        }
    }

    private void SpawnFog()
    {
        Vector3 fogSpawnPos = transform.position + transform.forward * fogDistanceSpawn;
        fogSpawnPos.y = 3.5f;

        GameObject fogClone = Instantiate(fogPrefab, fogSpawnPos, Quaternion.identity);
        fogClone.transform.parent = gameObject.transform.parent;

        Destroy(this.gameObject);
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
