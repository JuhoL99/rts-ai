using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject scout;
    public GameMap map;

    private void Start()
    {
        map = GameObject.Find("Game Map").GetComponent<GameMap>();
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Instantiate(scout, map.gameMap[0, 0].transformLoc, Quaternion.identity);
        }
    }
}

