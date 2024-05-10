using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 targetPosition;
    public List<Enemy> enemyList;
    public event EventHandler OnSomeoneReachedGoal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        targetPosition = new Vector3(19, 9, 0);
        enemyList = new List<Enemy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(enemyPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.SetTargetPosition(targetPosition);
            enemy.OnReachedGoal += RemoveEnemyFromList;
            enemyList.Add(enemy);
        }
        if(enemyList.Count == 0)
        {
            //Debug.Log("wave finished");
        }
    }

    // Event handler method to remove the enemy from the list
    private void RemoveEnemyFromList(object sender, System.EventArgs e)
    {
        Enemy enemyToRemove = sender as Enemy;
        if (enemyToRemove != null)
        {
            Debug.Log("removed itself");
            enemyList.Remove(enemyToRemove);
            Destroy(enemyToRemove.gameObject);
            OnSomeoneReachedGoal(this, EventArgs.Empty);
        }
    }
}
