using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    private Grid<Node> grid;
    public Node previousNode;
    public int x { get; private set; }
    public int y { get; private set; }

    public int gCost;
    public int hCost;
    public int fCost;
    public int moveCost;

    public bool isWall;
    public bool isTurret;

    int heapIndex;

    public Node(Grid<Node> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWall = false;
        moveCost = 1;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public Vector2 getXY()
    {
        return new Vector2(x, y);
    }
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
