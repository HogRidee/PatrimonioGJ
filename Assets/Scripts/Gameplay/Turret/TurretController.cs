using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TurretController : MonoBehaviour
{
    [Header("Targeting")]
    public float range = 15f;
    public string targetTag = "Player";

    [Header("Aiming")]
    public float rotationSpeed = 5f;

    [Header("Shooting")]
    public float fireRate = 1f;
    public float switchInterval = 5f;

    [Header("Warning")]
    public GameObject warningPrefab;
    public float warningTime = 1f;
    public float warningOffset = 2f;

    [Header("Startup Delay")]
    public float initialWarningDelay = 3f;
    private float startupTimer = 0f;


    private Transform target;
    private float fireCountdown;
    private float modeTimer;
    private bool useHomingShot;

    private bool hasWarned;
    private bool isShootingEnabled;

    void Update()
    {
        FindTarget();
        if (target == null) return;
        RotateTowards(target.position);
        if (startupTimer < initialWarningDelay)
        {
            startupTimer += Time.deltaTime;
            return;
        }
        FindTarget();
        if (target == null) return;
        RotateTowards(target.position);
        modeTimer += Time.deltaTime;
        if (modeTimer >= switchInterval)
        {
            useHomingShot = !useHomingShot;
            modeTimer = 0f;
            hasWarned = false;
            isShootingEnabled = false;
        }
        if (!hasWarned)
        {
            StartCoroutine(WarningAndEnableShooting());
            hasWarned = true;
            return;
        }
        if (!isShootingEnabled) return;
        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            ShootNow();
            fireCountdown = 1f / fireRate;
        }
    }

    private IEnumerator WarningAndEnableShooting()
    {
        if (warningPrefab && target != null)
        {
            Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
            Vector3 spawnPos = transform.position + (Vector3)(dir * warningOffset);
            var warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            Quaternion baseRot = transform.rotation;
            float extraZ = dir.x < 0 ? -90f : +90f;
            warning.transform.rotation = baseRot * Quaternion.Euler(0f, 0f, extraZ);
            Destroy(warning, warningTime);
        }
        yield return new WaitForSeconds(warningTime);
        isShootingEnabled = true;
    }


    private void RotateTowards(Vector3 worldPoint)
    {
        Vector3 dir = worldPoint - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle - 90f),
            Time.deltaTime * rotationSpeed
        );
    }

    private void ShootNow()
    {
        var pool = ProjectilePool.Instance;
        if (useHomingShot)
        {
            var proj = pool.Get();
            proj.transform.position = transform.position + transform.up * 1.5f;
            proj.transform.rotation = transform.rotation;
            proj.InitializeHoming(target);
        }
        else
        {
            var proj = pool.Get();
            Vector3 spawnPos = transform.position + transform.up * 1.5f;
            proj.InitializeStraight(spawnPos, target.position);
        }
    }

    private void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        float shortest = Mathf.Infinity;
        Transform best = null;
        foreach (var p in players)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);
            if (d < shortest)
            {
                shortest = d;
                best = p.transform;
            }
        }
        target = (best != null && shortest <= range) ? best : null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
