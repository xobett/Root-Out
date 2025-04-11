using Unity.VisualScripting;
using UnityEngine;

public class RedChibiPepper : CropBase
{
    [Header("RED CHIBI PEPPER SETTINGS")]
    [SerializeField] private GameObject redChibiExplosion;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource explosionSource;

    [SerializeField] private AudioClip warCryClip;
    [SerializeField] private AudioClip explosionClip;

    private bool audioPlayed;

    protected override void CropAttack()
    {
        PlayWarCry();

        HeadToEnemy();
    }

    protected override void Update()
    {
        base.Update();

        if (enemyPos == null)
        {
            audioPlayed = false;
        }
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
            GameObject explosionSource = Instantiate(new GameObject(), transform.position, Quaternion.identity);
            explosionSource.AddComponent<AudioSource>();

            AudioSource explosionAudioSource = explosionSource.GetComponent<AudioSource>();
            explosionAudioSource.clip = explosionClip;

            explosionAudioSource.playOnAwake = false;
            explosionAudioSource.loop = false;

            explosionAudioSource.Play();
            Destroy(explosionSource, 3);

            Vector3 spawnPos = transform.position;
            spawnPos.y = 0.6f;

            GameObject explosion = Instantiate(redChibiExplosion, spawnPos, Quaternion.identity);

            Destroy(explosion, 3);
            enemy.gameObject.GetComponent<AIHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
