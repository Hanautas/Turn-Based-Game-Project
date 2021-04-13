using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;
    private int value;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();
    }

    public void Heal()
    {
        value = UnityEngine.Random.Range(1, 8);
        gridCombatSystem.unitGridCombat.healthSystem.Heal(value);

        ActionLog.instance.OutputHealLog(gridCombatSystem.unitGridCombat.characterName, value);
        if (gridCombatSystem.unitGridCombat.healLines.Length > 0 && gridCombatSystem.isLogTimer == false)
        {
            ActionLog.instance.OutputCombatLine(gridCombatSystem.unitGridCombat.characterName, gridCombatSystem.unitGridCombat.healLines[UnityEngine.Random.Range(0, gridCombatSystem.unitGridCombat.healLines.Length)]);
            gridCombatSystem.logTimeRemaining = 5f;
            gridCombatSystem.isLogTimer = true;
        }

        Destroy(gameObject);
    }
}
