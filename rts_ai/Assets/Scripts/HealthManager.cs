using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth = 200;
    WaveSpawner enemyManager;
    void Start()
    {
        enemyManager = WaveSpawner.instance;
        enemyManager.OnEnemyEvent += HandleEnemyEvent;
        //enemyManager.OnSomeoneReachedGoal += EnemyReachedGoal;
        currentHealth = maxHealth;
        Debug.Log(currentHealth);

    }
    void Update()
    {
        
    }
    private void HandleEnemyEvent(object sender, EnemyEventArgs e)
    {
        if (e.EventType == EnemyEventType.ReachedGoal)
        {
            // Decrease health when an enemy reaches its goal
            currentHealth -= 5;
            Debug.Log("Health decreased by 5. Current health: " + currentHealth);
        }
    }
}
