using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SeedCoin : MonoBehaviour, IInteractable
{
    [SerializeField] private int coinsToAdd;

    [SerializeField] private Image recarga;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject seedModel;

    private void Update()
    {
        RotateModel();
    }

    private void RotateModel()
    {
        seedModel.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    public void OnInteract()
    {
        AddCoin();
    }

    private void AddCoin()
    {
        var playerInventory = GameManager.instance.playerInventoryHandler;
        playerInventory.AddSeedCoins(coinsToAdd);

        Destroy(gameObject);
    }
}
