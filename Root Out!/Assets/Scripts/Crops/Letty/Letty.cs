using Unity.VisualScripting;
using UnityEngine;

public class Letty : CropBase
{
    [Header("LETTY SETTINGS")]
    [SerializeField] private GameObject lettyShield;
    [SerializeField] private GameObject leafVfx;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leafClip;

    private bool audioPlayed;

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

        GameObject vfx = Instantiate(leafVfx, transform.position, Quaternion.identity);

        GameObject explosionSource = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        explosionSource.AddComponent<AudioSource>();

        AudioSource explosionAudioSource = explosionSource.GetComponent<AudioSource>();
        explosionAudioSource.clip = leafClip;

        explosionAudioSource.playOnAwake = false;
        explosionAudioSource.loop = false;

        explosionAudioSource.Play();

        Destroy(explosionSource, 3);
        Destroy(vfx, 2);

        if (GameObject.FindGameObjectWithTag("Letty Shield") == null)
        {
            SpawnShield();
            explosionAudioSource.Play();
            Destroy(gameObject);
        }
        else
        {
            lettyShield.GetComponent<LettyShield>().AddShieldLeaf();
            explosionAudioSource.Play();
            Destroy(gameObject);
        }
    }

    private void SpawnShield()
    {
        GameObject clone = Instantiate(lettyShield, playerPos.position, lettyShield.transform.rotation);
        clone.GetComponent<LettyShield>().GetPlayerReference(playerPos);
    }
}
