using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;

public class TilemapSystemTest : MonoBehaviour
{
    public TilemapVisual tilemapVisual;
    private TilemapSystem tilemap;
    private TilemapSystem.TilemapObject.TilemapSprite tilemapSprite;

    void Start()
    {
        tilemap = new TilemapSystem(20, 10, 1f, Vector3.zero);

        tilemap.SetTilemapVisual(tilemapVisual);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilitiesClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tilemapSprite = TilemapSystem.TilemapObject.TilemapSprite.None;
            Debug.Log("Tilemap Sprite: None.");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            tilemapSprite = TilemapSystem.TilemapObject.TilemapSprite.Ground;
            Debug.Log("Tilemap Sprite: Ground.");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            tilemapSprite = TilemapSystem.TilemapObject.TilemapSprite.Path;
            Debug.Log("Tilemap Sprite: Path.");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            tilemapSprite = TilemapSystem.TilemapObject.TilemapSprite.Dirt;
            Debug.Log("Tilemap Sprite: Dirt.");
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            tilemap.Save();
            Debug.Log("Tilemap Saved.");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            tilemap.Load();
            Debug.Log("Tilemap Loaded.");
        }
    }
}