using System.Collections;
using UnityEngine;
using Weapons;

public class ExplosionUpgrade : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject explosionPrefab;
    private WeaponsBase weapon;

    private void Start()
    {
        weapon =FindFirstObjectByType<WeaponsBase>();
    }

    public void OnInteract()
    {
       
        Debug.Log("Explosion Upgrade Activated" + weapon.damage);
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
