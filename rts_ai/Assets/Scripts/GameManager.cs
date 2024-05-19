using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance{  get; private set; }
    public Grid<Node> grid;
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject wall;
    public Pathfinding pathfinding;
    List<GameObject> tiles;
    private int width = 60;
    private int height = 30;

    [SerializeField] private GameObject enemy;
    private Enemy enemyScript;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        tiles = new List<GameObject>();
        pathfinding = new Pathfinding(width,height);
        grid = pathfinding.GetPathGrid();
        enemyScript = enemy.GetComponent<Enemy>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Instantiate(tile, grid.GetWorldPosition(x, y),Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 tPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(grid.GetGridElement(tPos) != null)
            {
                enemyScript.SetTargetPosition(tPos);
            }
        }
        /*Debug.Log((Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        bool live = !true;
        if(Input.GetMouseButtonDown(0) || live)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 coord = pathfinding.GetPathGrid().GetXY(mousePos);
            List<Node> path = pathfinding.FindPath(0, 0, (int)coord.x, (int)coord.y);
            if(path != null)
            {
                if(tiles != null)
                {
                    foreach (GameObject tileobject in tiles)
                    {
                        Destroy(tileobject);
                    }
                    tiles.Clear();
                }
                for(int i=0; i<path.Count -1;i++)
                {
                    GameObject tileObj = Instantiate(tile, new Vector3(path[i].x, path[i].y, 0), Quaternion.identity);
                    tileObj.GetComponent<TestTile>().setText($"{i}");
                    tiles.Add(tileObj);
                    //Debug.Log("tried drawing");
                    Debug.Log(path[i].x + " " + path[i].y);
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * 0.5f, new Vector3(path[i + 1].x, path[i + 1].y),Color.red);
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 coord = pathfinding.GetPathGrid().GetXY(mousePos);
            Node node = grid.GetGridElement(coord);
            node.isWall = true;
            Instantiate(wall,new Vector3(coord.x,coord.y,0), Quaternion.identity);
            
            /*Debug.Log(grid.GetGridElement(Camera.main.ScreenToWorldPoint(Input.mousePosition))?.getXY());
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        //Debug.Log(grid.GetGridElement(x,y) + ", "+ grid.GetGridElement(x, y).getXY());
                        GameObject tileObj = Instantiate(tile ,new Vector3(x,y,0),Quaternion.identity);
                        Vector2 coord = new Vector2(x,y);
                        tileObj.GetComponent<TestTile>().setText($"{coord.x},{coord.y}");
                    }
                }
            }
        }*/

    }
}
