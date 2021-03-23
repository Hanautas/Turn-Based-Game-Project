using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridCombatSystem.Utilities;

public class UnitGridCombat : MonoBehaviour {

    public Team team;

    private HealthSystem healthSystem;
    private MovePositionPathfinding movePosition;
    private State state;

    public HealthBar healthBar;
    public Text healthText;

    private GameObject unitIcon;
    private Sprite unitSprite;
    private GameObject deadPrefab;
    private Sprite deadSprite;

    [Header ("Stats")]
    public string characterName = "NAME";
    public int maxHealth;
    public int currentHealth;
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

    public enum Team {
        Blue,
        Red
    }

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    private void Awake() {
        movePosition = GetComponent<MovePositionPathfinding>();
        state = State.Normal;
        healthSystem = new HealthSystem(maxHealth);
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Start()
    {
        initiative = UnityEngine.Random.Range(1, 20);

        deadPrefab = Resources.Load("Dead") as GameObject;

        unitIcon = gameObject.transform.Find("Icon").gameObject;
        unitSprite = unitIcon.GetComponent<SpriteRenderer>().sprite;

        deadSprite = deadPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        currentHealth = healthSystem.health;
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + " / " + maxHealth;

        switch (state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition)
    {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition, () => {
            state = State.Normal;
            onReachedPosition();
        });
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat)
    {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    }

    public void AttackUnit(UnitGridCombat unitGridCombat, Action onAttackComplete)
    {
        state = State.Attacking;

        ShootUnit(unitGridCombat, () =>
        {
            state = State.Normal;
            onAttackComplete();
        });
    }

    private void ShootUnit(UnitGridCombat unitGridCombat, Action onShootComplete)
    {
        unitGridCombat.Damage(UnityEngine.Random.Range(1, 20), this);
        onShootComplete();
    }

    public void Damage(int damageAmount, UnitGridCombat unitGridCombat)
    {
        healthSystem.Damage(damageAmount);
        ActionLog.instance.OutputLog(unitGridCombat.characterName, damageAmount.ToString(), this.characterName);

        if (healthSystem.IsDead())
        {
            Debug.Log(gameObject.name + " is dead.");
            Destroy(gameObject);
            SpawnDead();
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void SpawnDead()
    {
        GameObject deadObject = Instantiate(deadPrefab, transform.position, Quaternion.identity) as GameObject;
        deadObject.GetComponent<SpriteRenderer>().sprite = unitSprite;
        deadObject.transform.Rotate(0, 0, -90);
    }

    public Vector3 GetPosition()
    {
        return new Vector3(transform.position.x, transform.position.y, -10);
    }

    public Team GetTeam()
    {
        return team;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat)
    {
        return unitGridCombat.GetTeam() != team;
    }
}