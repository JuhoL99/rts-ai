using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Grid<TGrid> //generic datatype, can use other than int
{
    private int width;
    private int height;
    private int cellSize;
    private TGrid[,] gridArray;
    private Vector3 origin;


    private GameObject debugTile;
    private GameObject[,] debugTiles;

    // Grid constructor, takes in map size and size of one map cell
    public Grid(int width, int height, int cellSize, Vector3 origin, GameObject debugTile)
    {
        this.debugTile = debugTile;

        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.gridArray = new TGrid[width, height];
        
        this.debugTiles = new GameObject[width, height];



        Debug.Log(width + " " + height);

        // Instantiate a tile to each gridArray coordinate
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                debugTiles[x, y] = Object.Instantiate(debugTile, GetWorldPosition(x, y), Quaternion.identity);
                debugTiles[x, y].transform.localScale = Vector3.one * cellSize;
            }
        }
    }
    // Get position of a cell in the world coordinates
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + origin;
    }
    // Get x and y of a cell in the grid coordinates
    private Vector2Int GetXY(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt((worldPos-origin).x/cellSize), Mathf.FloorToInt((worldPos-origin).y/cellSize));
    }
    // Set weight of a grid cell
    public void SetWeight(int x, int y, TGrid weight)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = weight;

            var text = weight.ToString();
            debugTiles[x, y].GetComponent<TestTile>().setText(text);
        }
    }
    // Set weight of a grid cell in the world position
    public void SetWeight(Vector3 worldPos, TGrid weight)
    {
        Vector2Int xy = GetXY(worldPos);
        int x = xy.x;
        int y = xy.y;
        SetWeight(x, y, weight);

    }
    // Get weight of a grid cel
    public TGrid GetWeight(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else { return default(TGrid); }
    }
    // Get weight of a grid cell in the world position
    public TGrid GetWeight(Vector3 worldPos)
    {
        Vector2Int xy = GetXY((Vector3)worldPos);
        return GetWeight(xy.x, xy.y);
    }
}
