using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropHandler : MonoBehaviour
{
    [Header("INVENTORY SETTINGS")]
    [SerializeField] private List<CropData> crops = new List<CropData>();
    [SerializeField] private CropData equippedCrop;

    [SerializeField] private int maxDrop_LettyCrops = 4;
    [SerializeField] private int maxDrop_SweetJackCrops = 2;
    [SerializeField] private int maxDrop_PepeHabaneroCrops = 2;
    [SerializeField] private int maxDrop_RedChibiCrops = 2;
    [SerializeField] private int maxDrop_CornyGuyCrops = 2;

    [Header("TYPES OF CROPS DROPPED")]
    private int droppedLettyCrops;
    private int droppedSweetJackCrops;
    private int droppedPepeHabaneroCrops;
    private int droppedRedChibiCrops;
    private int droppedCornyGuyCrops;

    [Header("CROP TIMERS")]
    private float timer_LettyCrop;
    private float timer_SweetJackCrop;
    private float timer_PepeHabaneroCrop;
    private float timer_RedChibiCrop;
    private float timer_CornyGuyCrop;

    [Header("TOTAL CROPS DROPPED")]
    [SerializeField] private int totalCropsDropped;
    [SerializeField] private int maxCropsOnField = 15;

    [Header("SPAWN CROP SETTINGS")]
    private const float spawnDistance = 1.5f;

    [Header("SELECTION WHEEL ICONS")]
    [SerializeField] private Image selectionWheel;
    [SerializeField] private Image previousCropIcon;
    [SerializeField] private Image equippedCropIcon;
    [SerializeField] private Image nextCropIcon;

    private const float rotationValue = 60f;
    [SerializeField, Range(0f, 5f)] private float rotationSpeed;
    private bool wheelIsRotating;


    void Start()
    {
        //equippedCrop = crops[3];
        //SetUIIcons();
    }

    // Update is called once per frame
    void Update()
    {
        DropCrop();
        CropSelection();
        PlayCropTimers();
    }

    private void DropCrop()
    {
        if (IsDropping() && totalCropsDropped < maxCropsOnField)
        {
            switch (equippedCrop.CropName)
            {
                case "Letty":
                    {
                        if (GetLettyShieldLeafs() == 3 || droppedLettyCrops >= maxDrop_LettyCrops || timer_LettyCrop > 0) return;
                        droppedLettyCrops++;
                        timer_LettyCrop = equippedCrop.CropCooldownTime;
                        break;
                    }

                case "Corny Guy":
                    {
                        if (droppedCornyGuyCrops >= maxDrop_CornyGuyCrops || timer_CornyGuyCrop > 0) return;
                        droppedCornyGuyCrops++;
                        timer_CornyGuyCrop = equippedCrop.CropCooldownTime;
                        break;
                    }

                case "Red Chibi Pepper":
                    {
                        if (droppedRedChibiCrops >= maxDrop_RedChibiCrops || timer_RedChibiCrop > 0) return;
                        droppedRedChibiCrops++;
                        timer_RedChibiCrop = equippedCrop.CropCooldownTime;
                        break;
                    }

                case "Pepe Habanero":
                    {
                        if (droppedPepeHabaneroCrops >= maxDrop_PepeHabaneroCrops || timer_PepeHabaneroCrop > 0) return;
                        droppedPepeHabaneroCrops++;
                        timer_PepeHabaneroCrop = equippedCrop.CropCooldownTime;
                        break;
                    }

                case "Sweet Jack O Pumpkin":
                    {
                        if (droppedSweetJackCrops >= maxDrop_SweetJackCrops || timer_SweetJackCrop > 0) return;
                        droppedSweetJackCrops++;
                        timer_SweetJackCrop = equippedCrop.CropCooldownTime;
                        break;
                    }
            }

            Debug.Log($"Cooldown time of {equippedCrop.CropName} is {equippedCrop.CropCooldownTime}");

            Vector3 spawnPosition = transform.position - transform.forward * spawnDistance;
            spawnPosition.y += 1;
            GameObject clone = Instantiate(equippedCrop.CropPrefab, spawnPosition, transform.rotation);
            totalCropsDropped++;
        }
    }
    private void PlayCropTimers()
    {
        timer_CornyGuyCrop -= Time.deltaTime;
        timer_LettyCrop -= Time.deltaTime;
        timer_PepeHabaneroCrop -= Time.deltaTime;
        timer_RedChibiCrop -= Time.deltaTime;
        timer_SweetJackCrop -= Time.deltaTime;
    }

    private void CropSelection()
    {
        if (!wheelIsRotating && MouseScrollWheelInput() != 0)
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

        Quaternion targetRotation = Quaternion.Euler(0, 0, selectionWheel.transform.eulerAngles.z + targetValue);

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

    public void UpdateDroppedCrop(string cropName)
    {
        switch (cropName)
        {
            case "Letty":
                {
                    droppedLettyCrops--;
                    break;
                }

            case "Corny Guy":
                {
                    droppedCornyGuyCrops--;
                    break;
                }

            case "Red Chibi Pepper":
                {
                    droppedRedChibiCrops--;
                    break;
                }

            case "Pepe Habanero":
                {
                    droppedPepeHabaneroCrops--;
                    break;
                }

            case "Sweet Jack O Pumpkin":
                {
                    droppedSweetJackCrops--;
                    break;
                }
        }

        totalCropsDropped--;
    }

    public void AddCrop(CropData crop)
    {

        //crops.Add(crop);
        crops.Remove(crops[0]);
        crops.Insert(0, crop);
        equippedCrop = crops[crops.IndexOf(crop)];
    }

    private void SetUIIcons()
    {
        previousCropIcon.sprite = equippedCrop == crops[0] ? crops[crops.Count - 1].CropIcon : crops[crops.IndexOf(equippedCrop) - 1].CropIcon;
        equippedCropIcon.sprite = equippedCrop.CropIcon;
        nextCropIcon.sprite = equippedCrop == crops[crops.Count - 1] ? crops[0].CropIcon : crops[crops.IndexOf(equippedCrop) + 1].CropIcon;
    }

    private int GetLettyShieldLeafs()
    {
        int currentShieldLeafs = 0;

        if (GameObject.FindGameObjectWithTag("Letty Shield"))
        {
            LettyShield lettyShield = GameObject.FindGameObjectWithTag("Letty Shield").GetComponent<LettyShield>();

            currentShieldLeafs = lettyShield.indexToUse;
        }

        return currentShieldLeafs;
    }

    #region Inputs

    private bool IsPressingShift() => Input.GetKey(KeyCode.LeftShift);
    private float MouseScrollWheelInput() => Input.GetAxis("Mouse ScrollWheel");
    private bool IsDropping() => Input.GetKeyDown(KeyCode.Q);

    #endregion
}
