using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugScript : MonoBehaviour
{
    public GameObject debugTile;
    Grid grid;
    void Start()
    {
        grid = new Grid(9,9,1,new Vector3(3,2,0), debugTile);
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
