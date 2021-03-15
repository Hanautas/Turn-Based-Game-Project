using UnityEngine;
using System.Collections;
using Pathfinding;

public class Blocker : MonoBehaviour
{
    public void Start () {
        var blocker = GetComponent<SingleNodeBlocker>();

        blocker.BlockAtCurrentPosition();
    }
}