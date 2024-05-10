using System;
using UnityEngine;

// Generic, can be used with different datatypes to create a grid of elements
public class Grid<TGrid>
{
    private int width;
    private int height;
    private int cellSize;
    private TGrid[,] gridArray;
    private Vector3 origin;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    // Event to notify when grid values change
    public class OnGridValueChangedEventArgs : EventArgs
    {
        // x and y of the changed grid cell
        public int x;
        public int y;
    }


    private GameObject debugTile;
    private GameObject[,] debugTiles;

    // Grid constructor, takes in map size, size of map cell, grid origin location
    public Grid(int width, int height, int cellSize, Vector3 origin, Func<Grid<TGrid>,int,int,TGrid> createGridElement)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.gridArray = new TGrid[width, height];

        Debug.Log("created a grid: " + width + " " + height);

        // Instantiate all grid elements
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridElement(this, x, y);
            }
        }

        // Debug show grid coordinates
        bool debugCoords = false;
        if(debugCoords)
        {
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height;y++)
                {
                    Debug.Log($"({x},{y})");
                }
            }
        }

    }
    // Method to get world position of a cell in coordinates
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + origin;
    }
    // Method to get grid coordinate from world position
    public Vector2Int GetXY(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt((worldPos-origin).x/cellSize), Mathf.FloorToInt((worldPos-origin).y/cellSize));
    }
    // Method to set grid element at specified coordinate
    public void SetGridElement(int x, int y, TGrid value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });

        }
    }
    // Method to set grid element at world position
    public void SetGridElement(Vector3 worldPos, TGrid value)
    {
        Vector2Int xy = GetXY(worldPos);
        int x = xy.x;
        int y = xy.y;
        SetGridElement(x, y, value);

    }
    // Method to trigger grid object changed event
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }
    // Method to get grid element at requested coordinate
    public TGrid GetGridElement(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else { return default(TGrid); } // Return default value if coordinates are out of bounds
    }

    // Method to get grid element at specified world position
    public TGrid GetGridElement(Vector3 worldPos)
    {
        Vector2Int xy = GetXY((Vector3)worldPos);
        return GetGridElement(xy.x, xy.y);
    }

    // Get methods for grid properties
    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public int GetCellSize() {  return cellSize; }
}
