using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    private GridCombatSystemMain gridCombatSystem;
    private int effectDuration;

    public GameObject effectRadius;
    public Vector3 effectScale;
    public Color effectColor;

    void Start()
    {
        gridCombatSystem = GameObject.Find("Systems/Grid Combat System").GetComponent<GridCombatSystemMain>();

        effectDuration = 10;
        effectRadius.transform.localScale = effectScale;
        effectRadius.GetComponent<SpriteRenderer>().color = effectColor;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Effect();
            effectDuration -= 1;
            Debug.Log("Area Effect duration: " + effectDuration);
        }

        if (effectDuration <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Effect()
    {

    }
}