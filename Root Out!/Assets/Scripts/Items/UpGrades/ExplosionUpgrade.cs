using System.Collections;
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
        StartCoroutine(Explosion());
        Destroy(gameObject);
    }

    IEnumerator Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        weapon.damage = 100;

        yield return new WaitForSeconds(3f);
    }
}
