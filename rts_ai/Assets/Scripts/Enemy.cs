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
    public event EventHandler<EnemyEventArgs> OnEnemyEvent;
    private int maxHealth = 10;
    private int currentHealth;

    private void Awake()
    {
        pathCoordinates = new List<Vector3>();
    }
    void Start()
    {
        buildManager = BuildManager.instance;
        buildManager.OnConstruction += RecalculatePathOnBuild;
        currentHealth = maxHealth;
    }
    void Update()
    {
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
            if(Vector3.Distance(GetCurrentPosition(), targetPosition) > .00001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                pathIndex++;
                if(pathIndex >= pathCoordinates.Count)
                {
                    if (OnEnemyEvent != null)
                        OnEnemyEvent(this, new EnemyEventArgs(EnemyEventType.ReachedGoal));
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
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if(currentHealth <= 0)
        {
            DeathSequence();
        }
    }
    private void DeathSequence()
    {

        if (OnEnemyEvent != null)
            OnEnemyEvent(this, new EnemyEventArgs(EnemyEventType.Died));
    }
}
