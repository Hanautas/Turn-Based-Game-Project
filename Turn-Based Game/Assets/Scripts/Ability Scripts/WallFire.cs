using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFire : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;
    public Transform[] fires;
    private int fireDuration;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();

        fires = GetComponentsInChildren<Transform>();
        fireDuration = 10;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            FireDamage();
            fireDuration -= 1;
            Debug.Log("Wall of Fire duration: " + fireDuration);
        }

        if (fireDuration <= 0)
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
                    if (firePos.transform.position == unit.transform.position)
                    {
                        unit.Damage(UnityEngine.Random.Range(5, 10), unit);
                    }
                }
            }
        }
    }
}