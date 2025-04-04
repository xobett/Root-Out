using System.Collections;
using UnityEngine;

public class SweetJack : CropBase
{
    [Header("SWEET JACK SETTINGS")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootSpawn;

    [SerializeField] private float nextShoot = 0.0f;
    [SerializeField] private float fireRate;

    private bool autoDestroyTimerActivated;

    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();

        if (enemyDetected && !arrivedToShootPos)
        {
            cropAnimCtrlr.SetBool("isRunning", true);
        }
        else if (enemyDetected && arrivedToShootPos || enemyPos == null)
        {
            cropAnimCtrlr.SetBool("isRunning", false);
        }
    }

    protected override void HeadToPlayer()
    {
        base.HeadToPlayer();

        gameObject.GetComponent<WeaponCoryGuy>().enabled = false;
    }

    protected override void CropAttack()
    {
        gameObject.GetComponent<WeaponCoryGuy>().enabled = true;

        base.HeadToShootingPos();

        StartCoroutine(StartAutoDestroy());

    }

    private IEnumerator StartAutoDestroy()
    {
        if (!autoDestroyTimerActivated)
        {
            autoDestroyTimerActivated = true;

            yield return new WaitForSeconds(10f);

            Destroy(gameObject);
        }
    }
}
