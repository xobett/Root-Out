using Unity.VisualScripting;
using UnityEngine;

public class Letty : CropBase
{
    [Header("SHIELD SETTINGS")]
    [SerializeField] private GameObject lettyShield;

    private bool shieldSpawned;

    protected override void CropAttack()
    {
        SpawnShield();
    }

    private void SpawnShield()
    {
        if (GameObject.FindGameObjectWithTag("Letty Shield") == null)
        {
            Debug.Log("Should instantiate letty shield");

            GameObject clone = Instantiate(lettyShield, playerPos.position, lettyShield.transform.rotation);
            clone.GetComponent<LettyShield>().GetPlayerReference(playerPos);
        }
    }
}
