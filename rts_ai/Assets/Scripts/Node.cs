using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Grid<Node> grid;
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public Node previousNode;

    public Node(Grid<Node> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
