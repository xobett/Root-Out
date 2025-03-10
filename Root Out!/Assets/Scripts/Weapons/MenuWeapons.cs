using UnityEngine;

public class MenuWeapons : MonoBehaviour
{
    public float spawnDistance;
    private void Update()
    {
        if (IsDroppingWeapon())
        {
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            spawnPosition.y += 1;
            GameObject clone = Instantiate(gameObject, spawnPosition, transform.rotation);

        }
    }
    void WeapnSelection()
    {
        if (MouseScrollWheel() > 0 && IsPressingShift())
        {
            // Select next weapon
        }
        else if (MouseScrollWheel() < 0 & IsPressingShift())
        {
            // Select previous weapon
        }
    }

    private bool IsPressingShift() => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    public float MouseScrollWheel() => Input.GetAxis("Mouse ScrollWheel");
    private bool IsDroppingWeapon() => Input.GetKeyDown(KeyCode.Q);
}
