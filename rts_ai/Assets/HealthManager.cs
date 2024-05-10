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
        enemyManager.OnSomeoneReachedGoal += EnemyReachedGoal;
        currentHealth = maxHealth;
        Debug.Log(currentHealth);

    }
    void Update()
    {
        
    }
    private void EnemyReachedGoal(object sender, EventArgs e)
    {
        currentHealth -= 5;
        Debug.Log(currentHealth);
    }
}
