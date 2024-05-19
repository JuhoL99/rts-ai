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
    public event EventHandler<EnemyEventArgs> OnEnemyEvent;

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
        targetPosition = new Vector3(59, 29, 0);
        enemyList = new List<Enemy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(enemyPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.SetTargetPosition(targetPosition);
            enemy.OnEnemyEvent += HandleEnemyEvent;
            enemyList.Add(enemy);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(enemyPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.SetTargetPosition(targetPosition);
            enemy.OnEnemyEvent += HandleEnemyEvent;
            enemyList.Add(enemy);

            yield return new WaitForSeconds(0.5f);
        }
    }
    private void HandleEnemyEvent(object sender, EnemyEventArgs e)
    {
        Enemy enemy = sender as Enemy;
        if (enemy != null)
        {
            if (e.EventType == EnemyEventType.ReachedGoal)
            {
                Debug.Log("Enemy reached goal.");
                OnEnemyEvent?.Invoke(this, e);
            }
            else if (e.EventType == EnemyEventType.Died)
            {
                Debug.Log("Enemy died.");
            }
            enemyList.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
}
public enum EnemyEventType
{
    ReachedGoal,
    Died
}
public class EnemyEventArgs : EventArgs
{
    public EnemyEventType EventType { get; private set; }

    public EnemyEventArgs(EnemyEventType eventType)
    {
        EventType = eventType;
    }
}
