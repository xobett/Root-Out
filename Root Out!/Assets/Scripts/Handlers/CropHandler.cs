using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CropHandler : MonoBehaviour
{
    [Header("INVENTORY SETTINGS")]
    [SerializeField] private List<CropData> crops = new List<CropData>();
    [SerializeField] private CropData equippedCrop;

    private const int maxLettyCrops = 4;
    private const int maxSweetJackCrops = 2;
    private const int maxPepeHabaneroCrops = 2;
    private const int maxRedChibiCrops = 2;
    private const int maxCornyGuyCrops = 2;

    [Header("TYPES OF CROPS DROPPED")]
    [SerializeField] private int droppedLettyCrops;
    [SerializeField] private int droppedSweetJackCrops;
    [SerializeField] private int droppedPepeHabaneroCrops;
    [SerializeField] private int droppedRedChibiCrops;
    [SerializeField] private int droppedCornyGuyCrops;

    [Header("TOTAL CROPS DROPPED")]
    [SerializeField] private int totalCropsDropped;
    private const int maxCropsOnField = 15;

    [Header("SPAWN CROP SETTINGS")]
    [SerializeField, Range(1, 5)] private float spawnDistance;

    [Header("SELECTION WHEEL ICONS")]
    [SerializeField] private Image selectionWheel;
    [SerializeField] private Image previousCropIcon;
    [SerializeField] private Image equippedCropIcon;
    [SerializeField] private Image nextCropIcon;

    private const float rotationValue = 60f;
    [SerializeField, Range(0f, 5f)] private float rotationSpeed;
    [SerializeField] private bool wheelIsRotating;


    void Start()
    {
        equippedCrop = crops[2];
        SetUIIcons();
    }

    // Update is called once per frame
    void Update()
    {
        DropCrop();
        CropSelection();
    }

    private void DropCrop()
    {
        if (IsDropping() && totalCropsDropped < maxCropsOnField)
        {
            switch (equippedCrop.CropName)
            {
                case "Letty":
                    {
                        if (droppedLettyCrops >= maxLettyCrops) return;
                        droppedLettyCrops++;
                        break;
                    }

                case "Corny Guy":
                    {
                        if (droppedCornyGuyCrops >= maxCornyGuyCrops) return;
                        droppedCornyGuyCrops++;
                        break;
                    }

                case "Red Chibi Pepper":
                    {
                        if (droppedRedChibiCrops >= maxRedChibiCrops) return;
                        droppedRedChibiCrops++;
                        break;
                    }

                case "Pepe Habanero":
                    {
                        if (droppedPepeHabaneroCrops >= maxPepeHabaneroCrops) return;
                        droppedPepeHabaneroCrops++;
                        break;
                    }

                case "Sweet Jack O Pumpkin":
                    {
                        if (droppedSweetJackCrops >= maxSweetJackCrops) return;
                        droppedSweetJackCrops++;
                        break;
                    }
            }

            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            spawnPosition.y += 1;
            GameObject clone = Instantiate(equippedCrop.CropPrefab, spawnPosition, transform.rotation);
            totalCropsDropped++;
        }
    }

    private void CropSelection()
    {
        if (!wheelIsRotating)
        {
            if (MouseScrollWheelInput() > 0 && IsPressingShift())
            {
                equippedCrop = equippedCrop == crops[crops.Count - 1] ? crops[0] : crops[crops.IndexOf(equippedCrop) + 1];
                StartCoroutine(RotateSelectionWheel(rotationValue));
            }
            else if (MouseScrollWheelInput() < 0 && IsPressingShift())
            {
                equippedCrop = equippedCrop == crops[0] ? crops[crops.Count - 1] : crops[crops.IndexOf(equippedCrop) - 1];
                StartCoroutine(RotateSelectionWheel(-rotationValue));
            }
            SetUIIcons();
        }
    }

    private IEnumerator RotateSelectionWheel(float targetValue)
    {
        wheelIsRotating = true;
        var selectionWheelRect = selectionWheel.GetComponent<RectTransform>(); 

        Quaternion targetRotation = Quaternion.Euler(0,0, selectionWheel.transform.eulerAngles.z + targetValue);

        Debug.Log(targetRotation.eulerAngles.z);

        float time = 0f;

        while (time < 1)
        {
            selectionWheelRect.rotation = Quaternion.Slerp(selectionWheelRect.rotation, targetRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        selectionWheelRect.rotation = targetRotation;

        wheelIsRotating = false;

        yield return null;
    }

    public void AddCrop(CropData crop)
    {
        crops.Add(crop);
        equippedCrop = crops[crops.IndexOf(crop)];
    }

    private void SetUIIcons()
    {
        previousCropIcon.sprite = equippedCrop == crops[0] ? crops[crops.Count - 1].CropIcon : crops[crops.IndexOf(equippedCrop) - 1].CropIcon;
        equippedCropIcon.sprite = equippedCrop.CropIcon;
        nextCropIcon.sprite = equippedCrop == crops[crops.Count - 1] ? crops[0].CropIcon : crops[crops.IndexOf(equippedCrop) + 1].CropIcon;
    }

    #region Inputs

    private bool IsPressingShift() => Input.GetKey(KeyCode.LeftShift);
    private float MouseScrollWheelInput() => Input.GetAxis("Mouse ScrollWheel");
    private bool IsDropping() => Input.GetKeyDown(KeyCode.Q);

    #endregion
}
