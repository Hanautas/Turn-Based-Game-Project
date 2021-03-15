using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridCombatSystem.Utilities;

public class GridTest : MonoBehaviour
{
    //public HeatMapVisual heatMapVisual;
    //public HeatMapBoolVisual heatMapBoolVisual;
    public HeatMapGenericVisual heatMapGenericVisual;

    //private GridSystem<HeatMapGridObject> grid;
    private GridSystem<StringGridObject> gridString;

    public float xPosition;
    public float yPosition;

    void Start()
    {
        //grid = new GridSystem<HeatMapGridObject>(40, 30, 1f, new Vector3(xPosition, yPosition), (GridSystem<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        gridString = new GridSystem<StringGridObject>(40, 30, 1f, new Vector3(xPosition, yPosition), (GridSystem<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));

        //heatMapVisual.SetGrid(grid);
        //heatMapBoolVisual.SetGrid(grid);
        //heatMapGenericVisual.SetGrid(grid);
    }

    void Update()
    {
        Vector3 position = UtilitiesClass.GetMouseWorldPosition();

        /*
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.A))
        {
            gridString.GetGridObject(position).AddLetter("A");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            gridString.GetGridObject(position).AddLetter("B");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            gridString.GetGridObject(position).AddLetter("C");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gridString.GetGridObject(position).AddNumber("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gridString.GetGridObject(position).AddNumber("2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gridString.GetGridObject(position).AddNumber("3");
        }
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private GridSystem<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatMapGridObject(GridSystem<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

public class StringGridObject
{
    private GridSystem<StringGridObject> grid;
    private int x;
    private int y;

    private string letters;
    private string numbers;

    public StringGridObject(GridSystem<StringGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter)
    {
        letters += letter;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void AddNumber(string number)
    {
        numbers += number;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return letters + "\n" + numbers;
    }
}