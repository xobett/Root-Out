using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [Header("GAMEOBJECT SETTINGS")]
    [SerializeField] private GameObject terrain;
    [SerializeField] private float distanceSpawn;

    [Header("DETECTION SETTINGS")]
    [SerializeField, Range(0, 100)] private int currentHealth;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask whatIsSunflower;

    [Range(1, 30)] private RaycastHit hit;

    void Start()
    {
        //RandomActivation();
        SunflowerNearDetection();
    }

    void Update()
    {
        TerrainSpawn();
    }

    private void TerrainSpawn()
    {
        if (IsSpawning())
        {
            Vector3 spawnPos = transform.position - transform.forward * distanceSpawn;
            spawnPos.y = 0;
            Instantiate(terrain, spawnPos, Quaternion.identity);
        }
    }

    private bool IsSpawning()
    {
        return Input.GetKeyDown(KeyCode.Space);
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

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
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
    public void DamageSunFlower(int damage)
    {
        currentHealth -= damage;   
    }
}
