using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour, IInteractable
{
    [Header("SPAWN SETTINGS")]
    public GameObject prefab;

    [SerializeField] private GameObject[] terrainPrefabs;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private float terrainDistanceSpawn = 11;
    [SerializeField] private float debugCubeDistance = 12;

    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Vector3 sunflowerCollisionCube = new Vector3(22, 2, 22);
    [SerializeField] private Vector3 groundCollisionCube = new Vector3(5, 2, 5);

    private bool sunflowerColliding;
    private bool groundColliding;

    [SerializeField] private bool activated;

    [Header("HEALTH SETTINGS")]
    [SerializeField, Range(0, 100)] private float currentHealth;
    private float maxHealth = 100f;

    [SerializeField] private TextMeshProUGUI porcentLife;

    private void Start()
    {
        BugDetection();
        //UpdateLife(); //Actualiza la UI de vida.
    }

    public void OnInteract()
    {
        TerrainSpawn();
    }

    private void TerrainSpawn()
    {
        if (!activated)
        {
            //Se crea un vector donde se almacenara la position donde se generara nuevo terreno.
            Vector3 spawnPos = transform.position + transform.forward * terrainDistanceSpawn;
            spawnPos.y = 0;

            int randomTerrainType = GenerateRandomTerrainType();

            //Se genera un nuevo terreno en la posicion creada.
            Instantiate(terrainPrefabs[randomTerrainType], spawnPos, terrainPrefabs[randomTerrainType].transform.rotation);

            //Tras instanciar el terreno, se autodestruye el girasol.
            Destroy(this.gameObject);
            
            activated = true;
        }
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

    private bool IsSpawning()
    {
        //Regresa si se esta spawneando un terreno.
        return Input.GetKeyDown(KeyCode.Space);
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
