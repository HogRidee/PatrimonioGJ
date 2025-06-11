using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public IObjectPool<Projectile> pool;

    [Header("General")]
    public float speed = 20f;
    public float lifeTime = 5f;

    [Header("Homing Settings")]
    public float homingDuration = 1f;     
    public float homingTurnSpeed = 120f;  

    private bool isHoming;
    private Transform target;
    private Vector3 direction; 
    private float timer;

    public void InitializeHoming(Transform target)
    {
        this.target = target;
        isHoming = true;
        timer = 0f;
        direction = (target.position - transform.position).normalized;
        transform.up = direction;
        gameObject.SetActive(true);
    }

    public void InitializeStraight(Vector3 startPos, Vector3 aimAt)
    {
        isHoming = false;
        timer = 0f;
        transform.position = startPos;
        direction = (aimAt - startPos).normalized;
        transform.up = direction;
        gameObject.SetActive(true);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            pool.Release(this);
            return;
        }
        if (isHoming)
        {
            if (timer >= homingDuration)
            {
                isHoming = false;
                direction = transform.up;
            }
            else if (target != null)
            {
                Vector2 currentDir = transform.up;
                Vector2 desiredDir = ((Vector2)target.position - (Vector2)transform.position).normalized;
                float angleDiff = Vector2.SignedAngle(currentDir, desiredDir);
                float maxAngleThisFrame = homingTurnSpeed * Time.deltaTime;
                float clampedAngle = Mathf.Clamp(angleDiff, -maxAngleThisFrame, maxAngleThisFrame);
                transform.Rotate(0f, 0f, clampedAngle);
            }
        }
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pool.Release(this);
        }
    }
}
