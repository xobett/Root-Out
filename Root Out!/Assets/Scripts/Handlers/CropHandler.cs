using System.Collections.Generic;
using UnityEngine;

public class CropHandler : MonoBehaviour
{
    [SerializeField] private List<CropData> crops = new List<CropData>();
    [SerializeField] private CropData equippedCrop;

    [SerializeField, Range(1, 5)] private float spawnDistance;

    void Start()
    {
        equippedCrop = crops[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDropping())
        {
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            spawnPosition.y += 1;
            GameObject clone = Instantiate(equippedCrop.CropPrefab, spawnPosition, transform.rotation);
        }
        CropSelection();
    }

    private void CropSelection()
    {
        if (MouseScrollWheelInput() > 0 && IsPressingShift())
        {
            equippedCrop = equippedCrop == crops[crops.Count - 1] ? crops[0] : crops[crops.IndexOf(equippedCrop) + 1];
        }
        else if (MouseScrollWheelInput() < 0 && IsPressingShift())
        {
            equippedCrop = equippedCrop == crops[0] ? crops[crops.Count - 1] : crops[crops.IndexOf(equippedCrop) - 1];
        }
    }

    public void AddCrop(CropData crop)
    {
        crops.Add(crop);
        equippedCrop = crops[crops.IndexOf(crop)];
    }

    #region Inputs

    private bool IsPressingShift() => Input.GetKey(KeyCode.LeftShift);
    private float MouseScrollWheelInput() => Input.GetAxis("Mouse ScrollWheel");
    private bool IsDropping() => Input.GetKeyDown(KeyCode.Q);

    #endregion
}
