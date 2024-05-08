using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestDebugScript : MonoBehaviour
{
    /*public GameObject debugTile;
    Grid<int> grid;
    Grid<bool> grid3;
    Grid<StringObject> stringGrid;
    void Start()
    {
        stringGrid = new Grid<StringObject>(5,5,2,new Vector3(12,0,0), debugTile,(Grid<StringObject> g,int x,int y)=>new StringObject(g,x,y));
        grid = new Grid<int>(9,9,1,new Vector3(10,10,0), debugTile, (Grid<int> g, int x, int y) => 0);
        Grid<int> grid2 = new Grid<int>(2, 3, 2, new Vector3(0, 0, 0), debugTile, (Grid<int> g, int x, int y) => 10);
        grid3 = new Grid<bool>(3, 3, 2, new Vector3(4, 4, 0), debugTile, (Grid<bool> g, int x, int y) => true);
    }
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetGridElement(Camera.main.ScreenToWorldPoint(Input.mousePosition), 10);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(stringGrid.GetGridElement(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            stringGrid.GetGridElement(mousePos).AddLetter("A");
        }
        
    }

}
public class StringObject
{
    private Grid<StringObject> grid;
    private int x;
    private int y;

    private string letters;

    public StringObject(Grid<StringObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
    }
    public void AddLetter(string letter)
    {
        letters += letter;
        grid.TriggerGridObjectChanged(x,y);
    }*/

}
