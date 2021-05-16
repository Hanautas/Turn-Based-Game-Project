using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovePosition : MonoBehaviour
{
    public GridCombatSystemMain system;

    void Update()
    {
        this.transform.position = system.aIMoveVector;
    }
}