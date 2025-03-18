using System;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour, IInteractable
{
    [Header("TERRAIN SPAWN SETTINGS")]
    [SerializeField] private GameObject[] terrainPrefabs;

    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject worldFog;

    private GameObject cloneFog;

    private const float fogDistanceSpawn = 21;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;

    private const float terrainDistanceSpawn = 21;
    private const float debugCubeDistance = 22;

    private Vector3 sunflowerCollisionCube = new Vector3(42, 2, 42);
    private Vector3 groundCollisionCube = new Vector3(10, 2, 10);

    private bool sunflowerColliding;
    private bool groundColliding;

    [Header("NAVMESH SETTINGS")]
    private NavMeshSurface navMeshSurface;

    [Header("HEALTH SETTINGS")]
    [SerializeField, Range(0, 100)] public float currentHealth;
    private float maxHealth = 100f;

    [SerializeField] private TextMeshProUGUI porcentLife;
    [SerializeField] private Image sunflowerLifebar;

    [Header("SUNFLOWER VISUAL GROWTH OPTIONS SETTINGS")]
    private SunflowerGrower sunflowerGrower;

    private void Start()
    {
        GetReferences();
        BugDetection();
        //UpdateLife(); //Actualiza la UI de vida.
    }

    public void OnInteract()
    {
        //var instance = GameManager.instance;

        //SpawnNewTerrain();
    }

    private void SpawnNewTerrain()
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

        int randomTerrainType = GenerateRandomTerrainType();

        ////Se genera un nuevo terreno en la posicion creada.
        Instantiate(terrainPrefabs[randomTerrainType], spawnPos, terrainPrefabs[randomTerrainType].transform.rotation);

        //Se actualiza el navmesh surface para que los enemigos naveguen por el.
        navMeshSurface.BuildNavMesh();

        ////Tras instanciar el terreno, se autodestruye el girasol.
        Destroy(this.gameObject);
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
        sunflowerGrower = transform.GetChild(1).GetComponent<SunflowerGrower>();
    }

    public void DamageSunFlower(float damage)
    {
        currentHealth -= damage;
        sunflowerLifebar.fillAmount = currentHealth / maxHealth;
        //UpdateLife();
    }

    private int GenerateRandomTerrainType()
    {
        return Random.Range(0, terrainPrefabs.Length);
    }

    //private void UpdateLife()
    //{
    //    porcentLife.text = $"{currentHealth}%";
    //}
}
