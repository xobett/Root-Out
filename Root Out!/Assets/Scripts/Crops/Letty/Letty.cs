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

        Destroy(vfx, 2);

        if (GameObject.FindGameObjectWithTag("Letty Shield") == null)
        {
            SpawnShield();
            PlayAudio();
            Destroy(gameObject);
        }
        else
        {
            lettyShield.GetComponent<LettyShield>().AddShieldLeaf();
            PlayAudio();
            Destroy(gameObject);
        }
    }

    private void PlayAudio()
    {
        if (!audioPlayed)
        {
            audioSource.clip = leafClip;
            audioSource.Play();

            audioPlayed = true;
        }
    }

    private void SpawnShield()
    {
        GameObject clone = Instantiate(lettyShield, playerPos.position, lettyShield.transform.rotation);
        clone.GetComponent<LettyShield>().GetPlayerReference(playerPos);
    }
}
