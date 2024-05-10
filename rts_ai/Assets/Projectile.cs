using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float projectileSpeed = 50f;
    void Start()
    {
        
    }
    void Update()
    {
        if (target == null)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.position, projectileSpeed * Time.deltaTime);
        float distanceToTarget = ((Vector2)target.transform.position - (Vector2)transform.position).magnitude;
        if (distanceToTarget < 0.1f)
        {
            Debug.Log("reached here");
            Enemy enemy = target.gameObject.GetComponent<Enemy>();
            enemy.Test();
            Destroy(gameObject);
        }
    }
    public void InitializeProjectile(Transform target)
    {
        this.target = target;

    }
}
