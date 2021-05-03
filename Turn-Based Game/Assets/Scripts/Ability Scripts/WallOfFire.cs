using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfFire : AbilityClass
{
    public Transform[] fires;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();
        owner = GetComponent<AbilityData>().abilityUnit;

        abilityName = "Wall of Fire";
        duration = 5;

        currentCount = gridCombatSystem.turnCount;

        fires = GetComponentsInChildren<Transform>();

        foreach (Transform firePos in fires)
        {
            foreach (UnitGridCombat unit in gridCombatSystem.unitGridCombatArray)
            {
                if (unit != null)
                {
                    if (firePos.transform.position == unit.transform.position )
                    {
                        unit.AbilityDamage(UnityEngine.Random.Range(5, 10), abilityName);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (currentCount != gridCombatSystem.turnCount)
        {
            FireDamage();

            if (gridCombatSystem.unitGridCombat == owner)
            {
                duration -= 1;
                Debug.Log(abilityName + " duration: " + duration);
            }

            currentCount = gridCombatSystem.turnCount;
        }

        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FireDamage()
    {
        foreach (Transform firePos in fires)
        {
            foreach (UnitGridCombat unit in gridCombatSystem.unitGridCombatArray)
            {
                if (unit != null)
                {
                    if (gridCombatSystem.unitGridCombat == unit && firePos.transform.position == unit.transform.position )
                    {
                        unit.AbilityDamage(UnityEngine.Random.Range(5, 10), abilityName);
                    }
                }
            }
        }
    }
}