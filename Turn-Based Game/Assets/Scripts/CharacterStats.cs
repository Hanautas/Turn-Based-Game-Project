using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header ("Stats")]
    public string characterName = "NAME";
    public int health;
    public int damage;
    public int armorClass;
    public int initiative;
    public int moveSpeed = 30;

    [Header ("Modifiers")]
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;
}
