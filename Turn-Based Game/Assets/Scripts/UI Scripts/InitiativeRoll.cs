using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeRoll : MonoBehaviour
{
    public GridCombatSystemMain gridCombatSystem;
    public UnitGridCombat[] playerUnitArray;
    public UnitGridCombat[] unitArray;

    public Transform contentObject;
    public GameObject waitText;
    public GameObject rollButton;
    private GameObject initiativePrefab;
    private GameObject initiativeObject;

    private string nameText;
    private int initiativeText;
    private Sprite unitSprite;

    private float desiredNumber;
    private float initialNumber;
    private float currentNumber;
    private float unitNumber;

    private bool startRoll = false;

    void Start()
    {
        StartCoroutine(GetPlayerUnits());
    }

    void Update()
    {
        if (startRoll == true)
        {
            foreach (UnitGridCombat unit in playerUnitArray)
            {
                initiativeText = unit.initiative;
                contentObject.transform.Find( "InitiativePanel" + unit.characterName).Find("Unit Initiative").GetComponent<Text>().text = initiativeText.ToString();
            }

            StartCoroutine(EndRoll());
        }

        unitArray = gridCombatSystem.unitGridCombatArray;
    }

    public IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(5);

        startRoll = false;
    }

    public IEnumerator GetPlayerUnits()
    {
        yield return new WaitForSeconds(0.1f);

        waitText.SetActive(false);
        rollButton.SetActive(true);
        playerUnitArray = gridCombatSystem.playerUnitGridCombatArray;
        initiativePrefab = Resources.Load("initiativePanel") as GameObject;

        foreach (UnitGridCombat unit in playerUnitArray)
        {
            nameText = unit.characterName;
            unitSprite = unit.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;

            initiativeObject = Instantiate(initiativePrefab, transform.position, Quaternion.identity) as GameObject;
            initiativeObject.transform.SetParent(contentObject, false);
        
            initiativeObject.transform.Find("Unit Name").GetComponent<Text>().text = nameText;
            initiativeObject.transform.Find("Unit Initiative").GetComponent<Text>().text = "0";
            initiativeObject.transform.Find("Unit Icon").GetComponent<Image>().sprite = unitSprite;

            initiativeObject.name = "InitiativePanel" + unit.characterName;
        }
    }

    public void RollInitiative()
    {
        startRoll = true;

        foreach (UnitGridCombat unit in unitArray)
        {
            unit.SetInitiative();
        }
    }
}