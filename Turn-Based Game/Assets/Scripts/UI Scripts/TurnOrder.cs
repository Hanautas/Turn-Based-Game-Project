using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{
    public GridCombatSystemMain gridCombatSystem;
    public UnitGridCombat[] unitArray;

    public Transform contentObject;
    public GameObject turnPrefab;

    private Text unitName;
    private Text unitInitiative;
    private Sprite unitIcon;

    private string nameText;
    private float initiativeText;
    private Sprite unitSprite;

    void Start()
    {

    }

    void Update()
    {

    }

    public void AddUnitTurn()
    {
        unitArray = gridCombatSystem.unitGridCombatArray;

        turnPrefab = Resources.Load("UnitTurnPanel") as GameObject;

        foreach (UnitGridCombat unit in unitArray)
        {
            nameText = unit.characterName;
            unitSprite = unit.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;
            if (unit.team == UnitGridCombat.Team.Blue)
            {
                initiativeText = unit.desiredNumber;
            }
            else
            {
                initiativeText = UnityEngine.Random.Range(1, 20);
            }

            GameObject turnObject = Instantiate(turnPrefab, transform.position, Quaternion.identity) as GameObject;
            turnObject.transform.SetParent(contentObject, false);

            turnObject.transform.Find("Unit Name").GetComponent<Text>().text = nameText;
            turnObject.transform.Find("Unit Initiative").GetComponent<Text>().text = initiativeText.ToString();
            turnObject.transform.Find("Unit Icon").GetComponent<Image>().sprite = unitSprite;
        }
    }
}