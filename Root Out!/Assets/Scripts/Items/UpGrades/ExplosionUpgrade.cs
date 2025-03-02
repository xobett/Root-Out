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
        weapon.damage += 100; // Aumenta el da�o del jugador en 100
        weapon.explosionPrefab = explosionPrefab; // Asigna el prefab de la explosi�n a las balas
        weapon.ActivateExplosionUpgrade(); // Activa la mejora de explosi�n
        Destroy(gameObject); // Destruye el objeto de mejora
    }
}
