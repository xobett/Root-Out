using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LettyShield : MonoBehaviour
{
    [Header("GENERAL SETTINGS")]
    [SerializeField, Range(0f, 4f)] private float rotationSpeed;
    [SerializeField] private List<GameObject> lettuceLeafs = new List<GameObject>();
    public int indexToUse = 0;

    [Header("SHIELD TIMER")]
    [SerializeField] private float shieldDuration;
    [SerializeField] private float timer_LettyShield; 

    private Transform player;

    private void Start()
    {
        timer_LettyShield = shieldDuration;
    }

    void Update()
    {
        RotateShield();
        FollowPlayer();
        SetShieldTimer();
    }

    [ContextMenu("Add leaf")]
    public void AddShieldLeaf()
    {
        if (indexToUse < 3)
        {
            indexToUse++;
            Debug.Log(indexToUse);
            lettuceLeafs[indexToUse].SetActive(true);
            timer_LettyShield = shieldDuration;
        }
    }

    private void RotateShield()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }

    private void SetShieldTimer()
    {
        timer_LettyShield -= Time.deltaTime;

        if (timer_LettyShield < 0)
        {
            Destroy(this.gameObject);
        }

    }

    public void GetPlayerReference(Transform playerPos)
    {
        player = playerPos;
    }
}
