using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 targetPosition;

    void Start()
    {
        targetPosition = new Vector3(8,8,0);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position + new Vector3(0.5f,0.5f,0),Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().SetTargetPosition(targetPosition);
        }
    }
}
