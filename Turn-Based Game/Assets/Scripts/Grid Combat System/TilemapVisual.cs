using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapVisual : MonoBehaviour
{
    [System.Serializable]
    public struct TilemapSpriteUV
    {
        public TilemapSystem.TilemapObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    public struct UVCoordinates
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    public TilemapSpriteUV[] tilemapSpriteUVArray;
    private GridSystem<TilemapSystem.TilemapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<TilemapSystem.TilemapObject.TilemapSprite, UVCoordinates> uvCoordinatesDictionary;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordinatesDictionary = new Dictionary<TilemapSystem.TilemapObject.TilemapSprite, UVCoordinates>();
        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray)
        {
            uvCoordinatesDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoordinates
            {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }

    public void SetGrid(TilemapSystem tilemap, GridSystem<TilemapSystem.TilemapObject> grid)
    {
        this.grid = grid;
        UpdateTilemapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;

        tilemap.OnLoaded += Tilemap_OnLoaded;
    }

    private void Tilemap_OnLoaded(object sender, System.EventArgs e)
    {
        updateMesh = true;
    }

    private void Grid_OnGridValueChanged(object sender, GridSystem<TilemapSystem.TilemapObject>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    private void UpdateTilemapVisual()
    {
        MeshUtilities.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                TilemapSystem.TilemapObject gridObject = grid.GetGridObject(x, y);
                TilemapSystem.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();
                
                Vector2 gridUV00, gridUV11;
                if (tilemapSprite == TilemapSystem.TilemapObject.TilemapSprite.None)
                {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    UVCoordinates uvCoordinates = uvCoordinatesDictionary[tilemapSprite];
                    gridUV00 = uvCoordinates.uv00;
                    gridUV11 = uvCoordinates.uv11;
                }
                MeshUtilities.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0f, quadSize, gridUV00, gridUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}