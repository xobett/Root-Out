using UnityEditor.UIElements;
using UnityEngine;

public class RedChibiPepper : CropBase
{
    [SerializeField] private TagField enemyTag;
    [SerializeField] private bool enemyDetected;

    protected override void Ability()
    {
        enemyDetected = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyDetected)
        {
            if (collision.gameObject.CompareTag("Mushroom Shooter") || collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Testing ability collision");
            } 
        }
    }
}
