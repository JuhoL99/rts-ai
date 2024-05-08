using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Grid<Node> grid;
    public Node previousNode;
    public int x { get; private set; }
    public int y { get; private set; }

    public int gCost;
    public int hCost;
    public int fCost;

    public Node(Grid<Node> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public Vector2 getXY()
    {
        return new Vector2(x, y);
    }
}
