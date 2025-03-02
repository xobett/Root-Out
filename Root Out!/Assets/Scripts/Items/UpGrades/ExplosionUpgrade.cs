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
        weapon.damage += 100; // Aumenta el daño del jugador en 100
        weapon.explosionPrefab = explosionPrefab; // Asigna el prefab de la explosión a las balas
        weapon.ActivateExplosionUpgrade(); // Activa la mejora de explosión
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
