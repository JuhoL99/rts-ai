using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    public static GameManager instance{  get; private set; }
    public Grid<Node> grid;
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject wall;
    public Pathfinding pathfinding;
    List<GameObject> tiles;
    private int width = 60;
    private int height = 30;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyPrefab;
    private Enemy enemyScript;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        tiles = new List<GameObject>();
        pathfinding = new Pathfinding(width,height);
        grid = pathfinding.GetPathGrid();
        enemyScript = enemy.GetComponent<Enemy>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Instantiate(tile, grid.GetWorldPosition(x, y),Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 tPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(grid.GetGridElement(tPos) != null)
            {
                
                enemyScript.SetTargetPosition(tPos);
            }
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            PathfindingTimer();
            //StartCoroutine(TestMovement());
            
        }
    }
    public IEnumerator TestMovement()
    {
        
        var targetPosition = new Vector3(1, 1, 0);
        var offset = new Vector3(0.5f, 0.5f, 0);
        enemyScript.SetTargetPosition(targetPosition);
        yield return new WaitForSeconds(2);
        //Debug.Log($"target: {targetPosition},enemy: {enemyScript.transform.position - offset}");
        Assert.AreApproximatelyEqual(targetPosition.x, (enemyScript.transform.position - offset).x);
        Assert.AreApproximatelyEqual(targetPosition.y, (enemyScript.transform.position - offset).y);
        Assert.AreEqual(targetPosition, enemyScript.transform.position);
    }
    public void PathfindingTimer()
    {
        int width = 5;
        int height = 5;
        for(int i = 1; i < 350; i++)
        {
            pathfinding = new Pathfinding(width*i, height*i);
            grid = pathfinding.GetPathGrid();
            var targetPosition = new Vector3(width*i-1, height*i-1, 0);

            // Instantiate a enemy gameobject, calculate its path and destroy it
            GameObject go = Instantiate(enemyPrefab, new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            go.GetComponent<Enemy>().SetTargetPosition(targetPosition); 
            Destroy(go);
        }
        
    }
}
