using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [Header("GAMEOBJECT SETTINGS")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private float distanceSpawn;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask whatIsSunflower;

    [Header("HEALTH SETTINGS")]
    [SerializeField, Range(0, 100)] private float currentHealth;

    [SerializeField] private RaycastHit hit;

    public bool testing;

    private void Start()
    {
        //RandomActivation();
        BugDetection();
        SunflowerNearDetection();
    }
    void Update()
    {
        TerrainSpawn();
        //BugDetection();
    }

    private void TerrainSpawn()
    {
        if (IsSpawning())
        {
            Vector3 spawnPos = transform.position + transform.forward * distanceSpawn;
            spawnPos.y = 0;
            Instantiate(terrain, spawnPos, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private bool IsSpawning()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void BugDetection()
    {
        Vector3 cubePos = transform.position + transform.forward * 12;

        testing = Physics.CheckBox(cubePos, new Vector3(11,2,11), Quaternion.identity, whatIsSunflower);

        if (testing)
        {
            Destroy(this.gameObject);
        }
    }

    private void SunflowerNearDetection()
    {
        if (Physics.Raycast(transform.position, transform.forward * rayDistance, out hit, rayDistance, whatIsSunflower))
        {
            Debug.Log("Colliding with sunflower");
            Debug.Log(hit.collider.gameObject.name);

            Destroy(hit.collider.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not colliding with sunflower");
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        Gizmos.color = Color.magenta;

        //Vector3 newPos = transform.position + transform.forward * 11;
        //Gizmos.DrawWireCube(newPos, new Vector3(22, 1, 22));

        Gizmos.DrawWireCube(transform.position + transform.forward * 12, new Vector3(22,2,22));
    }

    private void RandomActivation()
    {
        float randomNumber = Random.Range(1, 5);

        Debug.Log(randomNumber.ToString());

        if (randomNumber == 2)
        {
            Destroy(gameObject);
        }
    }
    public void DamageSunFlower(float damage)
    {
        currentHealth -= damage;
    }
}
