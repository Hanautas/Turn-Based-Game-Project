using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionMove : MonoBehaviour
{
    private Vector3 mousePosition;
    public Transform playerTarget;

    public TilemapRenderer collisionTilemap;

    public bool isMoving;

    void Start()
    {
        collisionTilemap.enabled = false;
        isMoving = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMoving == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerTarget.transform.position = new Vector2(mousePosition.x, mousePosition.y);
            
            //Make an if for movement cost so isMoving is only false when movement is consumed.
            //Make an if so isMoving is false when using another action.
            collisionTilemap.enabled = false;
            isMoving = false;
        }
        if ( Input.GetKey(KeyCode.Escape))
        {
            collisionTilemap.enabled = false;
            isMoving = false;
        }
    }

    public void SetMove()
    {
        collisionTilemap.enabled = true;
        isMoving = true;
    }
}