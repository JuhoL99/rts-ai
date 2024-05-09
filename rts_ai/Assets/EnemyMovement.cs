using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Properties;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public Vector3 targetPosition;
    private Vector3 currentGoal;
        
    private float moveSpeed = 2f;

    private int pathIndex;
    private List<Vector3> pathCoordinates;

    BuildManager buildManager;
    bool spawned = false;


    void Start()
    {
        buildManager = BuildManager.instance;
        buildManager.OnConstruction += RecalculatePathOnBuild;
    }
    void Update()
    {
        if(spawned)
            Movement();
    }
    private Vector3 GetCurrentPosition()
    {
        return transform.position;
    }
    private void Movement()
    {
        if (pathCoordinates != null && pathIndex < pathCoordinates.Count)
        {
            targetPosition = pathCoordinates[pathIndex];
            if(Vector3.Distance(GetCurrentPosition(), targetPosition) > .05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                pathIndex++;
                if(pathIndex >= pathCoordinates.Count)
                {
                    StopMovement();
                }
            }
        }
    }
    private void StopMovement()
    {
        pathCoordinates = null;
    }
    private void RecalculatePathOnBuild(object sender, EventArgs e)
    {
        Debug.Log("this shouldnt run");
        if (pathCoordinates != null && pathCoordinates.Count > 0)
        {
            StopMovement();
            // Recalculate the path to the current goal
            SetTargetPosition(currentGoal);
        }
    }
    public void SetTargetPosition(Vector3 targetPosition)
    {
        spawned = true;
        pathCoordinates = new List<Vector3>();
        pathIndex = 0;
        currentGoal = targetPosition;
        pathCoordinates = Pathfinding.instance.FindPath(GetCurrentPosition(), targetPosition);
        if (pathCoordinates != null && pathCoordinates.Count > 1)
        {
            pathCoordinates.RemoveAt(0);
        }
    }
}
