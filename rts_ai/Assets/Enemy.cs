using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Properties;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Vector3 targetPosition;
    private Vector3 currentGoal;
        
    private float moveSpeed = 2f;

    private int pathIndex;
    private List<Vector3> pathCoordinates;

    BuildManager buildManager;
    public event EventHandler OnReachedGoal;

    private void Awake()
    {
        pathCoordinates = new List<Vector3>();
    }
    void Start()
    {
        buildManager = BuildManager.instance;
        buildManager.OnConstruction += RecalculatePathOnBuild;
    }
    void Update()
    {
        Movement();
    }
    public void Test()
    {
        Debug.Log("hit success");
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
                    if (OnReachedGoal != null)
                        OnReachedGoal(this, EventArgs.Empty);
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
        if (pathCoordinates != null && pathCoordinates.Count > 0)
        {
            StopMovement();
            // Recalculate the path to the current goal
            SetTargetPosition(currentGoal);
        }
    }
    public void SetTargetPosition(Vector3 targetPosition)
    {
        pathIndex = 0;
        currentGoal = targetPosition;
        pathCoordinates = Pathfinding.instance.FindPath(GetCurrentPosition(), targetPosition);
        if (pathCoordinates != null && pathCoordinates.Count > 1)
        {
            // Remove the first coordinate which is enemy location
            pathCoordinates.RemoveAt(0);
        }
    }
}
