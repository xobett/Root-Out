using System.Collections;
using UnityEngine;

public class PepeHabanero : CropBase
{
    [Header("PEPE HABANERO SETTINGS")]
    [SerializeField] private GameObject cropModel;

    [SerializeField] private GameObject nukePrefab;
    [SerializeField] private float nukeRadius;

    private bool reachedExplosionPos;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip nukeClip;
    [SerializeField] private AudioClip warCryClip;

    protected override void CropAttack()
    {
        if (!reachedExplosionPos)
        {
            base.HeadToEnemy();   
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
        }
    }

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Mushroom Shooter") || enemy.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        cropAnimCtrlr.SetTrigger("Explode");

        reachedExplosionPos = true;

        audioSource.clip = warCryClip;
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        GameObject nukeExplosion = Instantiate(nukePrefab, transform.position, Quaternion.identity);

        Destroy(cropModel);

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, nukeRadius, whatIsEnemy);

        foreach(Collider enemyCollider in enemyColliders)
        {
            enemyCollider.GetComponent<AIHealth>().TakeDamage(damage);
        }

        Destroy(nukeExplosion, 7);

        audioSource.clip = nukeClip;
        audioSource.Play();

        while(audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    void PlayNukeSound()
    {
        audioSource.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nukeRadius);
    }
}
