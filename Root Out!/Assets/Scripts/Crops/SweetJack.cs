using UnityEngine;

public class SweetJack : CropBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    protected override void Attack()
    {

    }
    protected override void FollowPlayer()
    {
        base.FollowPlayer();
    }
}
