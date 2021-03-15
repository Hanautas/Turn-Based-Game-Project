using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<GameObject> objectsInRange = new List<GameObject>();
    public Collider2D enemyCollider;
    public bool isTriggered;

    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        isTriggered = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isTriggered = true;
        if (other.gameObject.tag == "Player")
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isTriggered = false;
        if (other.gameObject.tag == "Player")
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}