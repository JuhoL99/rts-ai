using System;
using UnityEngine;

public class Grid<TGrid> //generic datatype, can use other than int
{
    private int width;
    private int height;
    private int cellSize;
    private TGrid[,] gridArray;
    private Vector3 origin;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }


    private GameObject debugTile;
    private GameObject[,] debugTiles;

    // Grid constructor, takes in map size and size of one map cell
    public Grid(int width, int height, int cellSize, Vector3 origin, Func<Grid<TGrid>,int,int,TGrid> createGridElement)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.gridArray = new TGrid[width, height];

        Debug.Log("created a grid: " + width + " " + height);

        // Instantiate a tile to each gridArray coordinate
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridElement(this, x, y);
            }
        }

        bool debugMode = false;
        if(debugMode)
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
    // Get position of a cell in the world coordinates
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + origin;
    }
    // Get x and y of a cell in the grid coordinates
    public Vector2Int GetXY(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt((worldPos-origin).x/cellSize), Mathf.FloorToInt((worldPos-origin).y/cellSize));
    }
    // Set weight of a grid cell
    public void SetGridElement(int x, int y, TGrid value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });

        }
    }
    // Set weight of a grid cell in the world position
    public void SetGridElement(Vector3 worldPos, TGrid value)
    {
        Vector2Int xy = GetXY(worldPos);
        int x = xy.x;
        int y = xy.y;
        SetGridElement(x, y, value);

    }
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }
    // Get weight of a grid cel
    public TGrid GetGridElement(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else { return default(TGrid); }
    }
    // Get weight of a grid cell in the world position
    public TGrid GetGridElement(Vector3 worldPos)
    {
        Vector2Int xy = GetXY((Vector3)worldPos);
        return GetGridElement(xy.x, xy.y);
    }
    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
}
