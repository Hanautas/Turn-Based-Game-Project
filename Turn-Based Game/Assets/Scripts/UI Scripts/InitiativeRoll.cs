using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeRoll : MonoBehaviour
{
    public Transform contentObject;
    public GameObject initiativePrefab;

    public GameObject[] objectArray;
    public UnitGridCombat[] playerUnitArray;
    private UnitGridCombat tempUnit;

    public GameObject[] contentArray;
    public GameObject[] nameArray;
    public GameObject[] iconArray;
    public GameObject[] rollArray;

    private Sprite unitSprite;
    private string nameText;
    private int rollText;

    void Start()
    {
        initiativePrefab = Resources.Load("initiativePanel") as GameObject;

        foreach (GameObject name in nameArray)
        {
            //nameArray = gameObject.transform.Find("Name").gameObject;
        }

        objectArray = GameObject.FindGameObjectsWithTag("Player");
        playerUnitArray = new UnitGridCombat[objectArray.Length];
        for (int i = 0; i < objectArray.Length; i++)
        {
            playerUnitArray[i] = objectArray[i].GetComponent<UnitGridCombat>();
        }
    }

    void Update()
    {
        
    }

    public void AddUnitInitiative()
    {
        GameObject initiativeObject = Instantiate(initiativePrefab, transform.position, Quaternion.identity) as GameObject;
        initiativeObject.transform.SetParent(contentObject, false);
        
        initiativeObject.GetComponent<SpriteRenderer>().sprite = unitSprite;
    }

    public void RollInitiative()
    {
        for (int i = 0; i < playerUnitArray.Length; i++)
        {
            int rng = Random.Range(0, playerUnitArray.Length);
            tempUnit = playerUnitArray[rng];
            playerUnitArray[rng] = playerUnitArray[i];
            playerUnitArray[i] = tempUnit;
        }
    }
}