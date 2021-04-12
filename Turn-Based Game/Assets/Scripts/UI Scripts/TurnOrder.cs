using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{
    public GridCombatSystemMain gridCombatSystem;

    public Transform contentObject;
    private GameObject turnPrefab;

    private Text unitName;
    private Text unitInitiative;
    private Sprite unitIcon;

    private string nameText;
    private float initiativeText;
    private Sprite unitSprite;

    public void AddUnitTurn()
    {
        turnPrefab = Resources.Load("UnitTurnPanel") as GameObject;

        foreach (UnitGridCombat unit in gridCombatSystem.unitGridCombatArray)
        {
            nameText = unit.characterName;
            unitSprite = unit.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;
            initiativeText = unit.desiredNumber;

            GameObject turnObject = Instantiate(turnPrefab, transform.position, Quaternion.identity) as GameObject;
            turnObject.transform.SetParent(contentObject, false);

            turnObject.transform.Find("Unit Name").GetComponent<Text>().text = nameText;
            turnObject.transform.Find("Unit Initiative").GetComponent<Text>().text = initiativeText.ToString();
            turnObject.transform.Find("Unit Icon").GetComponent<Image>().sprite = unitSprite;

            turnObject.name = "UnitTurnPanel " + unit.name;
        }
    }

    public void SortUnitTurn()
    {
        foreach (UnitGridCombat unit in gridCombatSystem.unitGridCombatArray)
        {
            for (int i = 0; i < gridCombatSystem.unitGridCombatArray.Length; i++)
            {
                if (gridCombatSystem.unitGridCombatArray[i].characterName == unit.characterName)
                {
                    contentObject.transform.Find("UnitTurnPanel " + unit.name).SetSiblingIndex(i);
                }
            }
        }
    }

    public void ActiveUnitTurn()
    {
        foreach (UnitGridCombat unit in gridCombatSystem.unitGridCombatArray)
        {
            if (unit == gridCombatSystem.unitGridCombat)
            {
                // Make background color blue
                contentObject.transform.Find("UnitTurnPanel " + unit.name).GetComponent<Image>().color = new Color32(100, 200, 255, 255);
            }
            else if (unit != null)
            {
                // Make background color grey
                contentObject.transform.Find("UnitTurnPanel " + unit.name).GetComponent<Image>().color = new Color32(225, 225, 225, 255);
            }
        }
    }
}