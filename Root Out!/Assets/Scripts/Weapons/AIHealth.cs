using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float actualHealth;
    [SerializeField, Range(0, 100)] private float maxHealth;

    [SerializeField] private Slider lifeBar;

    public static int enemiesDefeated = 0; // Variable estática para contar los enemigos derrotados

    [SerializeField] private GameObject deathVfx;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;

    public void TakeDamage(float damage)
    {
        Debug.Log("El personaje " + name + " recibio daño");
        actualHealth -= damage;
        lifeBar.value = actualHealth / maxHealth;

        if (actualHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Mataste a " + name);
        enemiesDefeated++; // Incrementar el contador de enemigos derrotados

        audioSource.clip = deathClip;
        audioSource.Play();

        GameObject vfx = Instantiate(deathVfx, transform.position, Quaternion.identity);

        Destroy(vfx, 2);

        Destroy(gameObject);
    }
}
