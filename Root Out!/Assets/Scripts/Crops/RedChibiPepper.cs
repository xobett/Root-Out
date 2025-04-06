using Unity.VisualScripting;
using UnityEngine;

public class RedChibiPepper : CropBase
{
    [Header("RED CHIBI PEPPER SETTINGS")]
    [SerializeField] private GameObject redChibiExplosion;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip warCryClip;
    [SerializeField] private AudioClip explosionClip;

    private bool audioPlayed;

    protected override void CropAttack()
    {
        PlayWarCry();

        HeadToEnemy();
    }

    private void PlayWarCry()
    {
        if (!audioPlayed)
        {
            audioSource.clip = warCryClip;
            audioSource.Play();

            audioPlayed = true;
        }
    }

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();

        if (isFollowingEnemy)
        {
            cropAnimCtrlr.SetBool("isRunning", true);
        }
        else
        {
            cropAnimCtrlr.SetBool("isRunning", false);
            audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Mushroom Shooter") || enemy.gameObject.CompareTag("Enemy"))
        {
            audioSource.clip = explosionClip;
            audioSource.Play();

            Vector3 spawnPos = transform.position;
            spawnPos.y = 0.6f;

            GameObject explosion = Instantiate(redChibiExplosion, spawnPos, Quaternion.identity);

            Destroy(explosion, 3);
            enemy.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
