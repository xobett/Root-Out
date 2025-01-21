using UnityEngine;

public class Sunflower : MonoBehaviour
{
    [Header("GAMEOBJECT SETTINGS")]
    [SerializeField] private GameObject terrain;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnNewTerritory();
    }

    private void SpawnNewTerritory()
    {
        if (IsSpawning())
        {
            Debug.Log(transform.forward.normalized);
            Vector3 spawnPos = transform.forward;
            Instantiate(terrain, spawnPos, Quaternion.identity);
        }
    }

    private bool IsSpawning()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
