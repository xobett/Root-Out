using Unity.VisualScripting;
using UnityEngine;

public class Letty : CropBase
{
    [Header("SHIELD SETTINGS")]
    [SerializeField] private GameObject lettyShield;

    private bool shieldSpawned;

    protected override void CropAttack()
    {
        Shield();
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
    }

    private void Shield()
    {
        GameObject lettyShield = GameObject.FindGameObjectWithTag("Letty Shield");

        if (GameObject.FindGameObjectWithTag("Letty Shield") == null)
        {
            SpawnShield();
            Destroy(gameObject);
        }
        else
        {
            lettyShield.GetComponent<LettyShield>().AddShieldLeaf();
            Destroy(gameObject);
        }
    }

    private void SpawnShield()
    {
        GameObject clone = Instantiate(lettyShield, playerPos.position, lettyShield.transform.rotation);
        clone.GetComponent<LettyShield>().GetPlayerReference(playerPos);
    }
}
