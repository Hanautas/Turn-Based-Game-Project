using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;

    public GameObject abilityContent;

    private UnitGridCombat currentUnit;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();
    }

    void Update()
    {
        if (currentUnit != gridCombatSystem.unitGridCombat)
        {
            ClearContent();
            UpdateContent();

            currentUnit = gridCombatSystem.unitGridCombat;
        }
        
    }

    public void ClearContent()
    {
        for (int i = 0; i < abilityContent.transform.childCount; i++)
        {
            Transform child = abilityContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void UpdateContent()
    {
        
    }
}