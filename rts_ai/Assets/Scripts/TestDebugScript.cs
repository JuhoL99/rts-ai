using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugScript : MonoBehaviour
{
    public GameObject debugTile;
    Grid<int> grid;
    void Start()
    {
        grid = new Grid<int>(9,9,1,new Vector3(10,10,0), debugTile);
        Grid<int> grid2 = new Grid<int>(2, 3, 2, new Vector3(0, 0, 0), debugTile);
        Grid<bool> grid3 = new Grid<bool>(3, 3, 2, new Vector3(4, 4, 0), debugTile);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetWeight(Camera.main.ScreenToWorldPoint(Input.mousePosition), 10);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetWeight(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        
    }

}
