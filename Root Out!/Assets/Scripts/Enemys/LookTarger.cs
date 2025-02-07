using UnityEngine;

public class LookTarger : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sunFlower;


    private void Update()
    {
        LookAt();
    }
    void LookAt()
    {
        if(Detection())
        {
            transform.LookAt(player);
        }
        else
        {
            transform.LookAt(sunFlower);
        }
    }
    bool Detection()
    {
        return Physics.CheckSphere(transform.position, 10f);
    }
}
