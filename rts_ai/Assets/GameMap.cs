using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    [SerializeField] private int mapWidth = 4;
    [SerializeField] private int mapHeight = 4;
    public MapSquare[,] gameMap;
    public GameObject mapSquare;
    private void Start()
    {
        gameMap = new MapSquare[mapWidth, mapHeight];
        InitializeMap();

    }
    private void InitializeMap()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                gameMap[i, j] = new MapSquare(0, new Vector2(i, j));
                Instantiate(mapSquare, gameMap[i, j].transformLoc, Quaternion.identity);
            }
        }
    }

    private void Update()
    {

        Debug.Log($"{gameMap[0, 1].weight}");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameMap[0, 1].weight++;
        }
    }
}
public class MapSquare
{
    public float weight;
    public Vector2 transformLoc;

    public MapSquare(float weight, Vector2 transformLoc)
    {
        this.weight = weight;
        this.transformLoc = transformLoc;
    }
}
