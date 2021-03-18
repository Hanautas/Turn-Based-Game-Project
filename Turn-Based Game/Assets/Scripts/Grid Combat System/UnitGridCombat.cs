using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridCombatSystem.Utilities;

public class UnitGridCombat : MonoBehaviour {

    public Team team;

    //private Character_Base characterBase;
    private HealthSystem healthSystem;
    //private GameObject selectedGameObject;
    private MovePositionPathfinding movePosition;
    private State state;
    //private World_Bar healthBar;

    public int unitMaxHealth;
    public int unitCurrentHealth;
    public HealthBar healthBar;
    public Text healthText;

    public int healthDamageAmount;

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
        //characterBase = GetComponent<Character_Base>();
        //selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<MovePositionPathfinding>();
        //SetSelectedVisible(false);
        state = State.Normal;
        healthSystem = new HealthSystem(unitMaxHealth);
        //healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(10, 1.3f), Color.grey, Color.red, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
        //healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        healthBar.SetMaxHealth(unitMaxHealth);
    }

    /*
    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        healthBar.SetSize(healthSystem.GetHealthNormalized());
    }
    */

    private void Update()
    {
        unitCurrentHealth = healthSystem.health;
        healthBar.SetHealth(unitCurrentHealth);
        healthText.text = unitCurrentHealth + " / " + unitMaxHealth;

        switch (state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }

    /*
    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
    */

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
        unitGridCombat.Damage(UnityEngine.Random.Range(1, 20));
        onShootComplete();
    }

    public void Damage(int damageAmount)
    {
        healthDamageAmount = damageAmount;
        healthSystem.Damage(damageAmount);
        Debug.Log(gameObject.name + " took " + damageAmount + " damage.");
        if (healthSystem.IsDead())
        {
            Debug.Log(gameObject.name + " is dead.");
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
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