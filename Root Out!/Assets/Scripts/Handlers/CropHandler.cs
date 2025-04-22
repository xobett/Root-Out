using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropHandler : MonoBehaviour
{
    [Header("INVENTORY SETTINGS")]
    [SerializeField] private List<CropData> crops = new List<CropData>();

    [SerializeField] private CropData previousCrop;
    [SerializeField] private CropData equippedCrop;
    [SerializeField] private CropData nextCrop;

    [SerializeField] private GameObject[] cropCards;
    private int cardToActivate = 0;

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

    [Header("TOTAL CROPS DROPPED")]
    [SerializeField] private int totalCropsDropped;
    [SerializeField] private int maxCropsOnField = 15;

    [Header("SPAWN CROP SETTINGS")]
    [SerializeField] private GameObject spawnCropVfx;
    private const float spawnDistance = 1.5f;

    [Header("CROP ICON SETTINGS")]
    [SerializeField] private Image previousCropIcon;
    [SerializeField] private Image equippedCropIcon;
    [SerializeField] private Image nextCropIcon;

    [SerializeField] private TextMeshProUGUI previousCropText;
    [SerializeField] private TextMeshProUGUI equippedCropText;
    [SerializeField] private TextMeshProUGUI nextCropText;

    [SerializeField] private Image previousCropCBImage;
    [SerializeField] private Image equippedCropCBImage;
    [SerializeField] private Image nextCropCBImage;

    [Header("COOLDOWN ANIMATION SETTINGS")]
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float minimumValue;
    [SerializeField] private float maximumValue;
    private float lerpTime = 0.0f;

    void Update()
    {
        DropCrop();
        CropSelection();

        PlayCropTimers();
        DisplayCropsStats();
        CooldownAnimation();
    }

    private void DisplayCropsStats()
    {
        //if (previousCrop != null) previousCropText.text = $"+{previousCrop.cooldownBypass}";
        //if (equippedCrop != null) equippedCropText.text = $"+{equippedCrop.cooldownBypass}";
        //if (nextCrop != null) nextCropText.text = $"+{nextCrop.cooldownBypass}";



        if (previousCrop != null && previousCrop.cooldownBypass > 0) previousCropCBImage.gameObject.SetActive(true);
        else previousCropCBImage.gameObject.SetActive(false);

        if (equippedCrop != null && equippedCrop.cooldownBypass > 0) equippedCropCBImage.gameObject.SetActive(true);
        else equippedCropCBImage.gameObject.SetActive(false);

        if (nextCrop != null && nextCrop.cooldownBypass > 0) nextCropCBImage.gameObject.SetActive(true);
        else nextCropCBImage.gameObject.SetActive(false);

        if (previousCrop != null) previousCropText.text = previousCrop.cooldownBypass > 0 ? $"{previousCrop.cooldownBypass}" : string.Empty;
        if (equippedCrop != null) equippedCropText.text = equippedCrop.cooldownBypass > 0 ? $"{equippedCrop.cooldownBypass}" : string.Empty;
        if (nextCrop != null) nextCropText.text = nextCrop.cooldownBypass > 0 ? $"{nextCrop.cooldownBypass}" : string.Empty;

    }

    private void CooldownAnimation()
    {
        PreviousCropCooldown();
        EquippedCropCooldown();
        NextCropCooldown();

        lerpTime += fadeSpeed * Time.deltaTime;

        if (lerpTime > 1)
        {
            float temp = maximumValue;
            maximumValue = minimumValue;
            minimumValue = temp;
            lerpTime = 0f;
        }
    }

    private void DropCrop()
    {
        if (equippedCrop != null && IsDropping() && totalCropsDropped < maxCropsOnField) 
        {
            switch (equippedCrop.CropName)
            {
                case "Letty":
                    {
                        if (GetLettyShieldLeafs() == 3 || droppedLettyCrops >= maxDrop_LettyCrops || equippedCrop.timer > 0) return;
                        droppedLettyCrops++;
                        break;
                    }

                case "Corny Guy":
                    {
                        if (droppedCornyGuyCrops >= maxDrop_CornyGuyCrops || equippedCrop.timer > 0) return;
                        droppedCornyGuyCrops++;
                        break;
                    }

                case "Red Chibi Pepper":
                    {
                        if (droppedRedChibiCrops >= maxDrop_RedChibiCrops || equippedCrop.timer > 0) return;
                        droppedRedChibiCrops++;
                        break;
                    }

                case "Pepe Habanero":
                    {
                        if (droppedPepeHabaneroCrops >= maxDrop_PepeHabaneroCrops || equippedCrop.timer > 0) return;
                        droppedPepeHabaneroCrops++;
                        break;
                    }

                case "Sweet Jack O Pumpkin":
                    {
                        if (droppedSweetJackCrops >= maxDrop_SweetJackCrops || equippedCrop.timer > 0) return;
                        droppedSweetJackCrops++;
                        break;
                    }
            }

            if (equippedCrop.cooldownBypass > 0)
            {
                equippedCrop.cooldownBypass--;
            }
            else
            {
                equippedCrop.timer = equippedCrop.CropCooldownTime;
            }

            Debug.Log($"Cooldown time of {equippedCrop.CropName} is {equippedCrop.CropCooldownTime}");

            Vector3 spawnPosition = transform.position - transform.forward * spawnDistance;
            spawnPosition.y += 1;
            GameObject clone = Instantiate(equippedCrop.CropPrefab, spawnPosition, transform.rotation);
            GameObject vfx = Instantiate(spawnCropVfx, spawnPosition, Quaternion.identity);

            Destroy(vfx, 2);
            totalCropsDropped++;
        }
    }

    private void PlayCropTimers()
    {
        for (int i = 0; i < crops.Count; i++)
        {
            crops[i].timer -= Time.deltaTime;

            if (crops[i].timer > 0)
            {
                crops[i].cooldownAnimation = true;
            }
            else
            {
                crops[i].cooldownAnimation = false;
            }
        }
    }

    private void CropSelection()
    {
        if (MouseScrollWheelInput() != 0 && crops.Count > 1)
        {
            if (MouseScrollWheelInput() > 0 && IsPressingShift())
            {
                equippedCrop = equippedCrop == crops[crops.Count - 1] ? crops[0] : crops[crops.IndexOf(equippedCrop) + 1];
            }
            else if (MouseScrollWheelInput() < 0 && IsPressingShift())
            {
                equippedCrop = equippedCrop == crops[0] ? crops[crops.Count - 1] : crops[crops.IndexOf(equippedCrop) - 1];
            }
            SetUIIcons();
        }
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
        if (crops.Contains(crop))
        {
            crops[crops.IndexOf(crop)].cooldownBypass++;
        }
        else
        {
            crops.Add(crop);
            equippedCrop = crops[crops.IndexOf(crop)];
            equippedCrop.cooldownBypass = 0;
            equippedCrop.timer = 0;

            if (cardToActivate < cropCards.Length)
            {
                cropCards[cardToActivate].SetActive(true);
                cardToActivate++;
            }

            SetUIIcons();
        }
    }

    private void SetUIIcons()
    {
        if (crops.Count == 2)
        {
            previousCrop = equippedCrop == crops[0] ? crops[crops.Count - 1] : crops[crops.IndexOf(equippedCrop) - 1];
            previousCropIcon.sprite = previousCrop.CropIcon;
        }
        else if (crops.Count > 2)
        {
            previousCrop = equippedCrop == crops[0] ? crops[crops.Count - 1] : crops[crops.IndexOf(equippedCrop) - 1];
            nextCrop = equippedCrop == crops[crops.Count - 1] ? crops[0] : crops[crops.IndexOf(equippedCrop) + 1];

            previousCropIcon.sprite = previousCrop.CropIcon;
            nextCropIcon.sprite = nextCrop.CropIcon;
        }

        equippedCropIcon.sprite = equippedCrop.CropIcon;
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

    #region Cooldown animation methods
    void PreviousCropCooldown()
    {
        Color changingColor = Color.white;
        changingColor.a = minimumValue;

        if (previousCrop != null)
        {
            if (previousCrop.cooldownAnimation)
            {
                previousCropIcon.color = Color.Lerp(previousCropIcon.color, changingColor, lerpTime);
            }
            else
            {
                changingColor.a = 1;
                previousCropIcon.color = changingColor;
            }
        }
    }
    void EquippedCropCooldown()
    {
        Color changingColor = Color.white;
        changingColor.a = minimumValue;

        if (equippedCrop != null)
        {
            if (equippedCrop.cooldownAnimation)
            {
                equippedCropIcon.color = Color.Lerp(equippedCropIcon.color, changingColor, lerpTime);
            }
            else
            {
                changingColor.a = 1;
                equippedCropIcon.color = changingColor;
            }
        }
    }
    void NextCropCooldown()
    {
        Color changingColor = Color.white;
        changingColor.a = minimumValue;

        if (nextCrop != null)
        {
            if (nextCrop.cooldownAnimation)
            {
                nextCropIcon.color = Color.Lerp(nextCropIcon.color, changingColor, lerpTime);
            }
            else
            {
                changingColor.a = 1;
                nextCropIcon.color = changingColor;
            }
        }
    }

    #endregion

    #region Inputs
    private bool IsPressingShift() => Input.GetKey(KeyCode.LeftShift);
    private float MouseScrollWheelInput() => Input.GetAxis("Mouse ScrollWheel");
    private bool IsDropping() => Input.GetKeyDown(KeyCode.Q);
    #endregion
}
