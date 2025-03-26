using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float fogDetectionRadius;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blocked Path Fog"))
        {
            var otherFogPs = other.gameObject.GetComponent<ParticleSystem>();
            var main = otherFogPs.main;
            main.loop = false;

            Destroy(other.gameObject, 6);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue + Color.grey;
        Gizmos.DrawWireSphere(transform.position, fogDetectionRadius);
    }
}
