using System.Collections.Generic;
using UnityEngine;

public class LettyShield : MonoBehaviour
{
    [SerializeField, Range(0f, 4f)] private float rotationSpeed;
    [SerializeField] private List<GameObject> lettuceLeafs = new List<GameObject>();
    [SerializeField] private List<int> lettuceIndexUsed = new List<int>();
    private Transform player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateShield();
        FollowPlayer();
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

    public void GetPlayerReference(Transform playerPos)
    {
        player = playerPos;
    }


}
