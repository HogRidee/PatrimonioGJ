using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Targeting")]
    public float range = 15f;
    public string targetTag = "Player";

    [Header("Shooting")]
    public float fireRate = 1f;
    public float rotationSpeed = 5f;

    private Transform target;
    private float fireCountdown = 0f;

    void Update()
    {
        UpdateTarget();
        if (target == null){ 
            return;
        }
        RotateTowards(target.position);
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f/fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void UpdateTarget()
    {
        // SEARCH FOR NEAREST PLAYER
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDist = Mathf.Infinity;
        GameObject nearest = null;
        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                nearest = p;
            }
        }
        if (nearest != null && shortestDist <= range)
        {
            target = nearest.transform;
        }
        else
        {
            target = null;
        }
    }

    private void RotateTowards(Vector3 worldPoint)
    {
        Vector3 dir = worldPoint - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f), 
            Time.deltaTime * rotationSpeed);
    }

    private void Shoot()
    {
        Projectile proj = ProjectilePool.Instance.Get();
        Vector3 spawnPos = transform.position + transform.forward * 1.5f;
        proj.Initialize(spawnPos, target.position);
    }

    // VIEW RANGE
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
