using UnityEngine;

public class LookTarger : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        LookAt();
    }
    void LookAt()
    {
        transform.LookAt(player);
    }

}
