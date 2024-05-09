using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public event EventHandler OnConstruction;
    public static BuildManager instance;
    [SerializeField] private GameObject wall;
    [SerializeField] private Testing testing;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 coord = testing.pathfinding.GetPathGrid().GetXY(mousePos);
            Node node = testing.grid.GetGridElement(coord);
            Debug.Log(coord);
            if(!testing.pathfinding.IsPathBlockedByBuilding(new Vector2Int(0,0),new Vector2Int(19,9), node))
            {
                node.isWall = true;
                Instantiate(wall, new Vector3(coord.x, coord.y, 0), Quaternion.identity);
                if (OnConstruction != null)
                {
                    OnConstruction(this, EventArgs.Empty);
                }
            }
        }
    }
}
