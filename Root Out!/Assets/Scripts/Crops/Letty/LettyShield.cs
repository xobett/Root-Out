using System.Collections.Generic;
using UnityEngine;

public class LettyShield : MonoBehaviour
{
    [SerializeField, Range(0f, 4f)] private float rotationSpeed;
    [SerializeField] private List<GameObject> lettuceLeafs = new List<GameObject>();
    private int indexToUse;

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

    [ContextMenu("Add leaf")]
    public void AddShieldLeaf()
    {
        if (indexToUse < 3)
        {
            Debug.Log(lettuceLeafs.Count - 1);
            lettuceLeafs[++indexToUse].SetActive(true);
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

    public void GetPlayerReference(Transform playerPos)
    {
        player = playerPos;
    }
}
