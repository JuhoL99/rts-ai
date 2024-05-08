using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.Mathematics;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_DIAGONAL = 14;
    private const int MOVE_STRAIGHT = 10;

    private Grid<Node> grid;
    private List<Node> que;
    private List<Node> visited;

    public Pathfinding(int width, int height)
    {
        grid = new Grid<Node>(width, height, 1, Vector3.zero, (Grid<Node> g, int x, int y)=>new Node(g,x,y));
    }
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = grid.GetGridElement(startX, startY);
        Node endNode = grid.GetGridElement(endX, endY);
        que = new List<Node> { startNode };
        visited = new List<Node>();
        for(int x = 0; x < grid.GetWidth();  x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                Node node = grid.GetGridElement(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = DistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(que.Count > 0)
        {
            Node current = GetLowestFCostNode(que);
            if(current == endNode)
            {
                return CalculatePath(endNode);
            }
            que.Remove(current);
            visited.Add(current);

            foreach(Node neighbour in GetNeighbours(current))
            {
                if(visited.Contains(neighbour))
                {
                    continue;
                }
                int tentativeGScore = current.gCost + DistanceCost(current, neighbour);
                if(tentativeGScore < neighbour.gCost)
                {
                    neighbour.previousNode = current;
                    neighbour.gCost = tentativeGScore;
                    neighbour.hCost = DistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if(!que.Contains(neighbour))
                    {
                        que.Add(neighbour);
                    }
                }
            }
        }

        // que is empty
        return null;
    }
    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node current = endNode;
        while(current.previousNode != null)
        {
            path.Add(current.previousNode);
            current = current.previousNode;
        }
        path.Reverse();
        return path;
    }
    private List<Node> GetNeighbours(Node current)
    {
        List<Node> neighbourList = new List<Node> ();
        if(current.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(current.x - 1, current.y)); // left
            if(current.y-1 >= 0)
            {
                neighbourList.Add(GetNode(current.x-1, current.y-1)); // left down
            }
            if(current.y+1<grid.GetHeight())
            {
                neighbourList.Add(GetNode(current.x-1,current.y+1)); // left up
            }
        }
        if(current.x+1<grid.GetWidth())
        {
            neighbourList.Add(GetNode(current.x + 1, current.y)); // right
            if(current.y-1 >= 0)
            {
                neighbourList.Add(GetNode(current.x + 1, current.y-1)); // right down
            }
            if(current.y + 1<grid.GetHeight())
            {
                neighbourList.Add(GetNode(current.x+1, current.y+1)); // right up
            }
        }
        if(current.y -1 >= 0)
        {
            neighbourList.Add(GetNode(current.x,current.y-1)); // down
        }
        if(current.y + 1 <grid.GetHeight())
        {
            neighbourList.Add(GetNode(current.x, current.y+1)); // up
        }
        return neighbourList;
    }
    private int DistanceCost(Node a, Node b)
    {
        int xDistance = (int)Mathf.Abs(a.getXY().x - b.getXY().x);
        int yDistance = (int)Mathf.Abs(a.getXY().y - b.getXY().y);
        int remaining = xDistance - yDistance;
        return MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT * remaining;
    }
    private Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowestFCost = nodes[0];
        for(int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < lowestFCost.fCost)
            {
                lowestFCost = nodes[i];
            }
        }
        return lowestFCost;
    }
    public Grid<Node> GetPathGrid()
    {
        return grid;
    }
    private Node GetNode(int x, int y)
    {
        return grid.GetGridElement(x, y);
    }
}
