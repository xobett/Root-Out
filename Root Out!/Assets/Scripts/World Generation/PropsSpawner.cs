using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class PropsSpawner : MonoBehaviour
{
    [Header("PROP SETTINGS")]
    [SerializeField] private GameObject[] simpleProps;
    [SerializeField] private GameObject[] coverProps;


    [Header("PROP SPAWN POSITION SETTINGS")]
    [SerializeField] private Transform[] simplePropsPos;
    [SerializeField] private Transform[] coverPropsPos;

    private List<int> simplePropUsedPos = new List<int>();
    private List<int> coverPropUsedPos = new List<int>();

    void Start()
    {
        StartCoroutine(SpawnProps());
        StopCoroutine(SpawnProps());
    }

    private IEnumerator SpawnProps()
    {
        Debug.Log("Entra");

        for (int i = 0; i < simplePropsPos.Length; i++)
        {
            int randomSimpleProp = GenerateRandomProp("Simple");
            int randomSimplePropPos = GenerateRandomPropPos("Simple");

            while (simplePosUsed(randomSimplePropPos))
            {
                randomSimplePropPos = GenerateRandomPropPos("Simple");
                yield return null;
            }

            GameObject clone = Instantiate(simpleProps[randomSimpleProp], simplePropsPos[randomSimplePropPos].position, simpleProps[randomSimpleProp].transform.rotation);

            clone.transform.parent = gameObject.transform.parent;

            simplePropUsedPos.Add(randomSimplePropPos);
        }

        for (int i = 0; i < coverPropsPos.Length; i++)
        {
            int randomCoverProp = GenerateRandomProp("Cover");
            int randomCoverPropPos = GenerateRandomPropPos("Cover");

            while (coverPosUsed(randomCoverPropPos))
            {
                randomCoverPropPos = GenerateRandomPropPos("Cover");
                yield return null;
            }

            GameObject clone = Instantiate(coverProps[randomCoverProp], coverPropsPos[randomCoverPropPos].position, coverProps[randomCoverProp].transform.rotation);
            clone.transform.parent = gameObject.transform.parent;

            coverPropUsedPos.Add(randomCoverPropPos);
        }

        yield return null;
    }

    private bool simplePosUsed(int posToVerify)
    {
        return simplePropUsedPos.Contains(posToVerify);
    }

    private bool coverPosUsed(int posToVerify)
    {
        return coverPropUsedPos.Contains(posToVerify);
    }

    private int GenerateRandomProp(string propType)
    {
        int randomPropNumber = 0;

        switch (propType)
        {
            case "Simple":
                {
                    randomPropNumber = Random.Range(0, simpleProps.Length);
                    break;
                }

            case "Cover":
                {
                    randomPropNumber = Random.Range(0, coverProps.Length);
                    break;
                }
        }

        return randomPropNumber;
    }

    private int GenerateRandomPropPos(string propType)
    {
        int randomPosNumber = 0;
        switch (propType)
        {
            case "Simple":
                {
                    randomPosNumber = Random.Range(0, simplePropsPos.Length);
                    break;
                }

            case "Cover":
                {
                    randomPosNumber = Random.Range(0, coverPropsPos.Length);
                    break;
                }
        }

        return randomPosNumber;
    }
}
