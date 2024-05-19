using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    
    public event EventHandler OnConstruction;
    public static BuildManager instance;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject[] buildings;
    [SerializeField] private GameObject currentSelectedBuilding;
    private Dictionary<Vector2, GameObject> instantiatedBuildings = new Dictionary<Vector2, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentSelectedBuilding = buildings[0];
        gameManager = GameManager.instance;
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 coord = gameManager.pathfinding.GetPathGrid().GetXY(mousePos);
            Node node = gameManager.grid.GetGridElement(coord);
            if(node.isWall)
            {
                Destroy(instantiatedBuildings[coord]);
                instantiatedBuildings.Remove(coord);
                node.isWall = false;
                if (OnConstruction != null)
                {
                    OnConstruction(this, EventArgs.Empty);
                }
            }
            else
            {
                if(!gameManager.pathfinding.IsPathBlockedByBuilding(new Vector2Int(0,0),new Vector2Int(59,29), node))
                {
                    node.isWall = true;
                    GameObject newBuilding = Instantiate(currentSelectedBuilding, new Vector3(coord.x, coord.y, 0), Quaternion.identity);
                    instantiatedBuildings[coord] = newBuilding;
                    if (OnConstruction != null)
                    {
                        OnConstruction(this, EventArgs.Empty);
                    }
                }
            }
        }*/
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 coord = gameManager.pathfinding.GetPathGrid().GetXY(mousePos);
        Node node = gameManager.grid.GetGridElement(coord);
        if (Input.GetMouseButton(1) && !node.isWall)
        {
            if (!gameManager.pathfinding.IsPathBlockedByBuilding(new Vector2Int(0, 0), new Vector2Int(59, 29), node))
            {
                node.isWall = true;
                GameObject newBuilding = Instantiate(currentSelectedBuilding, new Vector3(coord.x, coord.y, 0), Quaternion.identity);
                instantiatedBuildings[coord] = newBuilding;
                if (OnConstruction != null)
                {
                    OnConstruction(this, EventArgs.Empty);
                }
            }
        }
        if(Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
        {
            if (node.isWall)
            {
                Destroy(instantiatedBuildings[coord]);
                instantiatedBuildings.Remove(coord);
                node.isWall = false;
                if (OnConstruction != null)
                {
                    OnConstruction(this, EventArgs.Empty);
                }
            }
        }
    }
    public void SetBuildingType(int bId)
    {
        currentSelectedBuilding = buildings[bId];
    }

}
