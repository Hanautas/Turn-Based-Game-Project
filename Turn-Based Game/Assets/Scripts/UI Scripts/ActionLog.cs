using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionLog : MonoBehaviour
{
    private HealthSystem healthSystem;
    public Text actionText;
    public GridCombatSystemMain gridCombatSystem;
    private string currentUnit;
    private string damageDealt;
    private string targetUnit;

    void Start()
    {
        actionText.text = "";
    }

    void Update()
    {
        gridCombatSystem = gridCombatSystem.GetComponent<GridCombatSystemMain>();
        currentUnit = gridCombatSystem.unitGridCombat.gameObject.name.ToString();
        damageDealt = gridCombatSystem.unitGridCombat.healthDamageAmount.ToString();
        targetUnit = gridCombatSystem.unitGridCombat.gameObject.name.ToString();
    }

    public void OutputLog()
    {
        actionText.text = currentUnit + " dealt " + damageDealt + " damage to " + targetUnit;
    }
}