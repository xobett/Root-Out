using UnityEngine;
using Weapons;

public class ExplosionUpgrade : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject explosionPrefab;
    private WeaponsBase weapon;

 
    private void Start()
    {
        weapon = FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
        if (weapon != null)
        {
            weapon.explosionUpgradeActivated = true; // Activar la mejora de explosi�n
            weapon.explosivePrefab = explosionPrefab; // Asignar el prefab de la explosi�n
        }
        Debug.Log("Explosion Upgrade Activated" + weapon.damage);
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
