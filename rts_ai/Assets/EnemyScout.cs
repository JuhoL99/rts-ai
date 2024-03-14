using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScout : MonoBehaviour
{
    public Vector2 currentLocation = Vector2.zero;
    void Start()
    {
        currentLocation = transform.position;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
        }
    }
}
