using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour, IInteractable
{
    [Header("TERRAIN SPAWN SETTINGS")]
    public GameObject prefab;

    [SerializeField] private GameObject[] terrainPrefabs;

    [Header("FOG SPAWN SETTINGS")]
    [SerializeField] private GameObject worldFog;
    [SerializeField] private const float fogDistanceSpawn = 21;

    private GameObject cloneFog;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private const float terrainDistanceSpawn = 21;
    [SerializeField] private const float debugCubeDistance = 22;

    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Vector3 sunflowerCollisionCube = new Vector3(22, 2, 22);
    [SerializeField] private Vector3 groundCollisionCube = new Vector3(5, 2, 5);

    private bool sunflowerColliding;
    private bool groundColliding;

    [SerializeField] private bool activated;

    [Header("HEALTH SETTINGS")]
    [SerializeField, Range(0, 100)] public float currentHealth;
    private float maxHealth = 100f;

    [SerializeField] private TextMeshProUGUI porcentLife;

    private void Start()
    {
        BugDetection();
        //UpdateLife(); //Actualiza la UI de vida.
    }

    public void OnInteract()
    {
        //var instance = GameManager.instance;
        //instance.GrowthSelectionEvent(this);

        var fogPs = cloneFog.GetComponent<ParticleSystem>();
        var main = fogPs.main;
        main.loop = false;

        Destroy(cloneFog, 6);

        //Se crea un vector donde se almacenara la position donde se generara nuevo terreno.
        Vector3 spawnPos = transform.position + transform.forward * terrainDistanceSpawn;
        spawnPos.y = 0;

        int randomTerrainType = GenerateRandomTerrainType();

        ////Se genera un nuevo terreno en la posicion creada.
        Instantiate(terrainPrefabs[randomTerrainType], spawnPos, terrainPrefabs[randomTerrainType].transform.rotation);

        ////Tras instanciar el terreno, se autodestruye el girasol.
        Destroy(this.gameObject);

        activated = true;
    }

    public void GrowSunflower(GrowthSelection growthType)
    {
        switch (growthType)
        {
            case GrowthSelection.Marvelous:
                {
                    Debug.Log("Marvelous way used");
                    break;
                }

            case GrowthSelection.Genuine:
                {
                    Debug.Log("Genuine way used");
                    break;
                }

            case GrowthSelection.Compelling:
                {
                    Debug.Log("Compelling way used");
                    break;
                }
        }
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

    public void DamageSunFlower(float damage)
    {
        currentHealth -= damage;
        UpdateLife();
    }

    private int GenerateRandomTerrainType()
    {
        return Random.Range(0, terrainPrefabs.Length);
    }

    private void UpdateLife()
    {
        porcentLife.text = $"{currentHealth}%";
    }
}
