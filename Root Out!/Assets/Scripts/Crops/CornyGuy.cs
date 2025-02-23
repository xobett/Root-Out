using UnityEngine;

public class CornyGuy : CropBase
{
    [SerializeField, Range(1f, 3f)] private float rotationSpeed;
    private void Update()
    {
        //transform.Rotate(0, rotationSpeed, 0);
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
