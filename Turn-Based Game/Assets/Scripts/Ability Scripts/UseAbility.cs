using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;

public class UseAbility : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;

    public bool isPlacing;
    public bool isRotated;

    public GameObject ability;
    public GameObject abilityPreview;
    private GameObject abilityInstantiated;
    private Vector3 position;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();

        isRotated = false;
    }

    void Update()
    {
        if (isPlacing == true)
        {
            position = new Vector2(Mathf.Round(UtilitiesClass.GetMouseWorldPosition().x), Mathf.Round(UtilitiesClass.GetMouseWorldPosition().y));
            abilityInstantiated.transform.position = position;

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isRotated == false)
                {
                    isRotated = true;
                    abilityInstantiated.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else
                {
                    isRotated = false;
                    abilityInstantiated.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }

            if (Input.GetMouseButtonDown(0) && UtilitiesClass.IsPointerOverUIObject() == false)
            {
                PlaceAbility();
                isPlacing = false;
                isRotated = false;
                Destroy(abilityInstantiated);
            }
        }
    }

    public void SetIsPlacing()
    {
        GameObject abilityPlacement = Instantiate(abilityPreview, position, Quaternion.identity) as GameObject;
        abilityInstantiated = abilityPlacement;
        isPlacing = true;
    }

    public void PlaceAbility()
    {
        GameObject abilityObject = Instantiate(ability, position, Quaternion.identity) as GameObject;

        abilityObject.GetComponent<AbilityData>().abilityUnit = gridCombatSystem.unitGridCombat;
        if (isRotated == true)
        {
            abilityObject.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            abilityObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}