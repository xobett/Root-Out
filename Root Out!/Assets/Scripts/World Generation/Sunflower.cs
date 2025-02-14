using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sunflower : MonoBehaviour
{
    [Header("GAMEOBJECT SETTINGS")]
    public GameObject prefab;

    [Header("DETECTION SETTINGS")]
    [SerializeField] private float terrainDistanceSpawn = 11;
    [SerializeField] private float debugCubeDistance = 12;

    [SerializeField] private LayerMask whatIsSunflower;
    [SerializeField] private LayerMask whatIsGround;

    private bool sunflowerColliding;
    private bool groundColliding;

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
        Debug.Log("Testing");
        //TerrainSpawn();
    }

    private void TerrainSpawn()
    {
        //Se crea un vector donde se almacenara la position donde se generara nuevo terreno.
        Vector3 spawnPos = transform.position + transform.forward * terrainDistanceSpawn;
        spawnPos.y = 0;
        //Se genera un nuevo terreno en la posicion creada.
        Instantiate(prefab, spawnPos, prefab.transform.rotation);

        //Tras instanciar el terreno, se autodestruye el girasol.
        Destroy(this.gameObject);
    }


    private void BugDetection()
    {
        //Se crea un vector donde se crearan las areas de deteccion de bugs.
        Vector3 cubePos = transform.position + transform.forward * debugCubeDistance;

        //Detecta si el area de generacion de terreno esta colisionando con otro girasol.
        sunflowerColliding = Physics.CheckBox(cubePos, new Vector3(11, 1, 11), Quaternion.identity, whatIsSunflower);

        //Detecta si el area de generacion de terreno esta colisionando con terreno.
        groundColliding = Physics.CheckBox(cubePos, new Vector3(2.5f, 1, 2.5f), Quaternion.identity, whatIsGround);

        //Si detecta que hay un escenario donde generara doble terreno.
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
        Gizmos.DrawWireCube(transform.position + transform.forward * debugCubeDistance, new Vector3(22, 2, 22));

        //Dibuja el cuadro de deteccion de terreno en area de generacion de terreno.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + transform.forward * debugCubeDistance, new Vector3(5, 2, 5));

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

    private void UpdateLife()
    {
        porcentLife.text = $"{currentHealth}%";
    }
}
