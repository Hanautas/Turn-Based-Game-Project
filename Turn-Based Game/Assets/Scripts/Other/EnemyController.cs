using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private Vector2 direction;

    private void Start()
    {
        target = FindTarget();
    }

    private void FixedUpdate()
    {
        //Make an if for turn order so enemy doesnt constantly chase.
        //Make an if for distance to target so enemy can attack instead of chase.
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            gameObject.transform.position = new Vector2(target.position.x, target.position.y);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.tag = "Untagged"; // Remove the tag so that FindTarget won't return it
            Destroy(collision.collider.gameObject);
            target = FindTarget();
        }
    }

    public Transform FindTarget()
    {
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Player");
        float minDistance = Mathf.Infinity;
        Transform closest;
         
        if (candidates.Length == 0)
            return null;
 
        closest = candidates[0].transform;
        for (int i = 1 ; i < candidates.Length ; ++i)
        {
            float distance = (candidates[i].transform.position - transform.position).sqrMagnitude;
 
            if (distance < minDistance)
            {
                closest = candidates[i].transform;
                minDistance = distance;
            }
        }
        return closest;
    }
}