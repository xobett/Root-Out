using UnityEngine;

public class BlockedPathFog : MonoBehaviour
{
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
}
