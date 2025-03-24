using System;
using System.Collections;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [Header("TERRAIN SPAWN SETTINGS")]
    [SerializeField] private GameObject[] terrainPrefabs;

    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject worldFog;

    private GameObject cloneFog;

    private const float fogDistanceSpawn = 22;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;

    private const float terrainDistanceSpawn = 22;
    private const float debugCubeDistance = 23;

    private Vector3 sunflowerCollisionCube = new Vector3(43, 2, 43);
    private Vector3 groundCollisionCube = new Vector3(11, 2, 11);

    private bool sunflowerColliding;
    private bool groundColliding;

    [Header("NAVMESH SETTINGS")]
    private NavMeshSurface navMeshSurface;

    [Header("HEALTH SETTINGS")]
    [SerializeField, Range(0, 100)] public float currentHealth;
    private float maxHealth = 100f;

    [SerializeField] private TextMeshProUGUI porcentLife;
 //   [SerializeField] private Image sunflowerLifebar;

    private void Start()
    {
        currentHealth = maxHealth;

        GetReferences();
        BugDetection();
    }

    public void StartGrowthSuccess()
    {
        StartCoroutine(CreateNewTerrain());
    }

    private IEnumerator CreateNewTerrain()
    {
        var fogPs = cloneFog.GetComponent<ParticleSystem>();
        var main = fogPs.main;
        main.loop = false;

        var fogCollider = cloneFog.transform.GetChild(0).gameObject;

        Destroy(fogCollider, 4.5f);
        Destroy(cloneFog, 6);

        //Se crea un vector donde se almacenara la position donde se generara nuevo terreno.
        Vector3 spawnPos = transform.position + transform.forward * terrainDistanceSpawn;
        spawnPos.y = 0;

        GameObject terrainToSpawn = GenerateRandomTerrainType();

        if (terrainToSpawn == terrainPrefabs[0] && GameManager.instance.finalHubCreated)
        {
            while (terrainToSpawn == terrainPrefabs[0])
            {
                terrainToSpawn = GenerateRandomTerrainType();
                Debug.Log("Was supposed to spawn final hub but it was already created");
                yield return null;
            } 
        }
        else if (terrainToSpawn == terrainPrefabs[0] && !GameManager.instance.finalHubCreated)
        {
            GameManager.instance.finalHubCreated = true;
            Debug.Log("Spawned final hub");
        }

        ////Se genera un nuevo terreno en la posicion creada.
        Instantiate(terrainToSpawn, spawnPos, terrainToSpawn.transform.rotation);

        //Se actualiza el navmesh surface para que los enemigos naveguen por el.
        navMeshSurface.BuildNavMesh();

        yield return null;
    }

    private void SpawnFog()
    {
        Vector3 fogSpawnPos = transform.position + transform.forward * fogDistanceSpawn;
        fogSpawnPos.y = 3.5f;

        cloneFog = Instantiate(worldFog, fogSpawnPos, Quaternion.identity);
        cloneFog.transform.parent = gameObject.transform.parent;
    }

    private void BugDetection()
    {
        //Se crea un vector donde se crearan las areas de deteccion de bugs.
        Vector3 cubePos = transform.position + transform.forward * debugCubeDistance;

        //Detecta si el area de generacion de terreno esta colisionando con otro girasol.
        sunflowerColliding = Physics.CheckBox(cubePos, sunflowerCollisionCube / 2, Quaternion.identity, whatIsSunflower);

        //Detecta si el area de generacion de terreno esta colisionando con terreno.
        groundColliding = Physics.CheckBox(cubePos, groundCollisionCube / 2, Quaternion.identity, whatIsGround);

        //Si detecta que hay un escenario donde se puede generar doble terreno.
        if (sunflowerColliding || groundColliding)
        {
            //Tras detectar positivo, se autodestruye el girasol.
            Destroy(this.gameObject);
        }
        else
        {
            SpawnFog();
        }
    }

    private void OnDrawGizmos()
    {
        //Dibuja el cuadro de deteccion de girasoles en area de generacion de terreno.
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + transform.forward * debugCubeDistance, sunflowerCollisionCube);

        //Dibuja el cuadro de deteccion de terreno en area de generacion de terreno.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + transform.forward * debugCubeDistance, groundCollisionCube);
    }

    private void GetReferences()
    {
        navMeshSurface = GameObject.FindGameObjectWithTag("Main Floor").GetComponent<NavMeshSurface>();
    }

    public void DamageSunFlower(float damage)
    {
        currentHealth -= damage;
      //  sunflowerLifebar.fillAmount = currentHealth / maxHealth;
        //UpdateLife();
    }

    private GameObject GenerateRandomTerrainType()
    {
        int randomTerrainIndex = Random.Range(0, terrainPrefabs.Length);

        return terrainPrefabs[randomTerrainIndex];
    }

    //private void UpdateLife()
    //{
    //    porcentLife.text = $"{currentHealth}%";
    //}
}
