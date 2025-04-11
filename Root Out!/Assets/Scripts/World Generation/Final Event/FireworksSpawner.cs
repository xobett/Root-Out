using UnityEngine;

public class FireworksSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireworksVfx;

    [SerializeField] private Transform[] spawnPoints;

    private Quaternion spawnRotation = Quaternion.Euler(-90f, 0, 0);

    public void SpawnEndingFireworks()
    {
        foreach (Transform spawnPosition in spawnPoints)
        {
            Instantiate(fireworksVfx, spawnPosition.position, spawnRotation);
        }
    }
}
