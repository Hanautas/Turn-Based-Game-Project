using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;
    private int value;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Grid Combat System").GetComponent<GridCombatSystemMain>();
    }

    public void Heal()
    {
        value = UnityEngine.Random.Range(1, 8);
        gridCombatSystem.unitGridCombat.healthSystem.Heal(value);

        ActionLog.instance.OutputHealLog(gridCombatSystem.unitGridCombat.characterName, value);

        Destroy(gameObject);
    }
}
