using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityClass : MonoBehaviour
{
    public GridCombatSystemMain gridCombatSystem;
    public UnitGridCombat owner;

    public string abilityName;
    public int duration;

    public int currentCount;
}