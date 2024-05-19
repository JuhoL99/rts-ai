using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class Pathfinding
{
    // pathfinding instance so it can be accessed everywhere
    public static Pathfinding instance { get; private set; }

    private const int MOVE_DIAGONAL = 14;
    private const int MOVE_STRAIGHT = 10;

    // predefined neighbor offsets for all 8 compass directions 
    private static readonly Vector2Int[] NeighborOffsets = {
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 1)
    };

    private Grid<Node> grid;
    private Heap<Node> openSet;
    private List<Node> closedSet;

    // pathfinding class constructor to initialize the grid and instance
    public Pathfinding(int width, int height)
    {
        instance = this;
        grid = new Grid<Node>(width, height, 1, Vector3.zero, (Grid<Node> g, int x, int y) => new Node(g,x,y));
    }

    // same function as below but returns a vector3 list instead which can be used for traveling in the world space
    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 goalWorldPos)
    {
        Vector2Int startXY = grid.GetXY(startWorldPos);
        Vector2Int endXY = grid.GetXY(goalWorldPos);
        List<Node> path = FindPath(startXY.x, startXY.y, endXY.x, endXY.y);
        if (path == null) return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Node node in path)
            {
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f);
            }
            return vectorPath;
        }
    }
    // A* pathfinding
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        // nullcheck for if you click outside the grid
        if (!IsWithinBounds(startX, startY) || !IsWithinBounds(endX, endY))
        {
            return null;
        }
        // initialize start and end nodes
        Node startNode = grid.GetGridElement(startX, startY);
        Node endNode = grid.GetGridElement(endX, endY);
        // initialize openset as heap and closedset as list
        openSet = new Heap<Node>(grid.GetWidth() * grid.GetHeight());
        closedSet = new List<Node>();
        // reset node data
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Node node = grid.GetGridElement(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.previousNode = null;
            }
        }
        // set startnode gcost, hcost and fcost
        startNode.gCost = 0;
        startNode.hCost = DistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        openSet.AddToHeap(startNode);
        // main A* algoritm loop
        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (openSet.GetItemCount() > 0)
        {
            //get lowest fcost node which is the root of the heap
            Node current = openSet.RemoveFirstHeapItem();
            if (current == endNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log("Pathfinding took: "+ sw.ElapsedMilliseconds+ " ms");
                string path = Application.persistentDataPath + "/pathfinding_times.txt";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(sw.ElapsedMilliseconds);
                }
                return CalculatePath(endNode);
            }
            closedSet.Add(current);
            foreach (Node neighbour in GetNeighbours(current))
            {
                // ignore walls and already searched nodes
                if (closedSet.Contains(neighbour) || neighbour.isWall)
                {
                    continue;
                }
                int estimatedPathCost = current.gCost + DistanceCost(current, neighbour);
                // check if the new path to the neighbor is cheaper than any previously found path
                if (estimatedPathCost < neighbour.gCost)
                {
                    neighbour.previousNode = current;
                    neighbour.gCost = estimatedPathCost;
                    neighbour.hCost = DistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();
                    if (!openSet.ContainsItem(neighbour))
                    {
                        openSet.AddToHeap(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        return null;
    }

    // check if building a node will block the path between startXY and endXY using BFS
    public bool IsPathBlockedByBuilding(Vector2Int startXY, Vector2Int endXY, Node nodeBeingBuilt)
    {
        Node startNode = grid.GetGridElement(startXY.x, startXY.y);
        Node endNode = grid.GetGridElement(endXY.x, endXY.y);
        if (startNode == nodeBeingBuilt)
        {
            return true;
        }
        // temporarily mark a grid cell as a walll
        bool originalWallStatus = nodeBeingBuilt.isWall;
        nodeBeingBuilt.isWall = true;

        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        visited.Add(startNode);
        queue.Enqueue(startNode);
        // BFS to check if a path exists from start to goal
        while (queue.Count != 0)
        {
            Node current = queue.Dequeue();

            // if path is found return false and let buildmanager mark the cell as a wall permanently
            if (current == endNode)
            {
                nodeBeingBuilt.isWall = originalWallStatus;
                return false;
            }

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (neighbour.isWall)
                {
                    visited.Add(neighbour);
                    continue;
                }
                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
        // path would be blocked by a wall
        nodeBeingBuilt.isWall = originalWallStatus;
        return true;
    }

    // backtrack from endnode checking previous nodes until one without previous is found to get a path and reverse it
    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node current = endNode;
        while (current.previousNode != null)
        {
            path.Add(current.previousNode);
            current = current.previousNode;
        }
        path.Reverse();
        return path;
    }
    // get a list of neighbor nodes for a node
    private List<Node> GetNeighbours(Node current)
    {
        List<Node> neighbourList = new List<Node>();
        // loop through neigbor offsets and check if they are inside the grid
        foreach (Vector2Int offset in NeighborOffsets)
        {
            int newX = current.x + offset.x;
            int newY = current.y + offset.y;
            if (IsWithinBounds(newX, newY))
            {
                neighbourList.Add(GetNode(newX, newY));
            }
        }

        return neighbourList;
    }
    // check whether coordinate is within grid bounds
    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < grid.GetWidth() && y >= 0 && y < grid.GetHeight();
    }
    // cost to move between 2 nodes
    private int DistanceCost(Node a, Node b)
    {
        int xDistance = (int)Mathf.Abs(a.getXY().x - b.getXY().x);
        int yDistance = (int)Mathf.Abs(a.getXY().y - b.getXY().y);
        int remaining = (int)Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT * remaining;
    }
    public Grid<Node> GetPathGrid()
    {
        return grid;
    }
    // return node at requested coordinate
    private Node GetNode(int x, int y)
    {
        return grid.GetGridElement(x, y);
    }
}
/* TEMPORARY REMOVE LATER JUST FOR TESTING
public class Pathfinding
{
    // Instance to make the class accessible anywhere
    public static Pathfinding instance { get; private set; }

    // Movement costs for diagonal and straight movement
    private const int MOVE_DIAGONAL = 14;
    private const int MOVE_STRAIGHT = 10;
    private static readonly Vector2Int[] NeighborOffsets = {
    new Vector2Int(-1, 0),  // left
    new Vector2Int(1, 0),   // right
    new Vector2Int(0, -1),  // down
    new Vector2Int(0, 1),   // up
    new Vector2Int(-1, -1), // left down
    new Vector2Int(-1, 1),  // left up
    new Vector2Int(1, -1),  // right down
    new Vector2Int(1, 1)    // right up
    };

    private Grid<Node> grid;
    private List<Node> openSet;
    private List<Node> closedSet;

    public Pathfinding(int width, int height)
    {
        instance = this;
        // Grid is initialized with Node instances as elements using a lambda expression
        grid = new Grid<Node>(width, height, 1, Vector3.zero, (Grid<Node> g, int x, int y) => new Node(g, x, y));
    }
    // Find and return a path of Vector3 positions from start position to end position
    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 goalWorldPos)
    {
        Vector2Int startXY = grid.GetXY(startWorldPos);
        Vector2Int endXY = grid.GetXY(goalWorldPos);
        List<Node> path = FindPath(startXY.x, startXY.y, endXY.x, endXY.y);
        if (path == null) return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Node node in path)
            {
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * 0.5f);
            }
            return vectorPath;
        }


    }
    // Find and return a path of Nodes from start to end
    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = grid.GetGridElement(startX, startY);
        Node endNode = grid.GetGridElement(endX, endY);
        openSet = new List<Node> { startNode };
        closedSet = new List<Node>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
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
        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (openSet.Count > 0)
        {
            Node current = GetLowestFCostNode(openSet);
            if (current == endNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log("Pathfinding took: " + sw.ElapsedMilliseconds + " ms");
                string path = Application.persistentDataPath + "/pathfinding_times.txt";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(sw.ElapsedMilliseconds);
                }
                return CalculatePath(endNode);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }
                if (neighbour.isWall)
                {
                    closedSet.Add(neighbour);
                    continue;
                }
                int tentativeGScore = current.gCost + DistanceCost(current, neighbour);
                if (tentativeGScore < neighbour.gCost)
                {
                    neighbour.previousNode = current;
                    neighbour.gCost = tentativeGScore;
                    neighbour.hCost = DistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }
    // Check if path would be blocked by building
    public bool IsPathBlockedByBuilding(Vector2Int startXY, Vector2Int endXY, Node nodeBeingBuilt)
    {

        Node startNode = grid.GetGridElement(startXY.x, startXY.y);
        Node endNode = grid.GetGridElement(endXY.x, endXY.y);
        // Check if player is trying to build on the spawn node
        if (startNode == nodeBeingBuilt)
        {
            return true;
        }

        // Store the original wall status of the node being built
        bool originalWallStatus = nodeBeingBuilt.isWall;
        nodeBeingBuilt.isWall = true;


        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();


        visited.Add(startNode);
        queue.Enqueue(startNode);

        // BFS to check if the path is blocked
        while (queue.Count != 0)
        {
            Node current = queue.Dequeue();
            if (current == endNode)
            {
                // Reset the wall status of the node being built
                nodeBeingBuilt.isWall = originalWallStatus;
                return false; // Path is not blocked
            }

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (neighbour.isWall)
                {
                    visited.Add(neighbour);
                    continue;
                }
                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
        // Reset the wall status of the node being built
        nodeBeingBuilt.isWall = originalWallStatus;
        return true; // Path is blocked
    }
    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node current = endNode;
        while (current.previousNode != null)
        {
            path.Add(current.previousNode);
            current = current.previousNode;
        }
        path.Reverse();
        return path;
    }
    private List<Node> GetNeighbours(Node current)
    {
        List<Node> neighbourList = new List<Node>();

        foreach (Vector2Int offset in NeighborOffsets)
        {
            int newX = current.x + offset.x;
            int newY = current.y + offset.y;
            if (IsWithinBounds(newX, newY))
            {
                neighbourList.Add(GetNode(newX, newY));
            }
        }

        return neighbourList;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < grid.GetWidth() && y >= 0 && y < grid.GetHeight();
    }
    private int DistanceCost(Node a, Node b)
    {
        int xDistance = (int)Mathf.Abs(a.getXY().x - b.getXY().x);
        int yDistance = (int)Mathf.Abs(a.getXY().y - b.getXY().y);
        int remaining = (int)Mathf.Abs(xDistance - yDistance);  //was int remaining = xDistance - yDistance; fixed now!
        return MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT * remaining;
    }
    private Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowestFCost = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
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
}*/
