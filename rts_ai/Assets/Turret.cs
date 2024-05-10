using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Turret : MonoBehaviour
{
    private float range = 5f;
    private float rotationSpeed = 250f;
    private Transform target;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private GameObject projectile;

    private float fireCooldown = 0.25f;
    private float lastFireTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        HandleRotation();
        if (TargetInRange() && CanFire())
        {
            HandleFiring();
        }
        if(!TargetInRange())
        {
            target = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    private void HandleRotation()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x)*Mathf.Rad2Deg-90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void HandleFiring()
    {

        if(target != null)
        {
            GameObject go = Instantiate(projectile, projectileSpawnPos.transform.position,Quaternion.identity);
            go.GetComponent<Projectile>().InitializeProjectile(target);
            lastFireTime = Time.time;
        }
    }
    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, target.position) < range;
    }
    private bool CanFire()
    {
        return Time.time - lastFireTime >= fireCooldown;
    }
    private void FindTarget()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, enemyLayer);
        if(hit.Length != 0)
        {
            target = hit[0].transform;
        }
    }
}
