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
    private Vector2 direction;  
    private float timer;

    public void InitializeHoming(Transform target)
    {
        this.target = target;
        isHoming = true;
        timer = 0f;
        Vector2 toTarget = (target.position - transform.position).normalized;
        direction = toTarget;
        AlignSpriteToDirection();  
        gameObject.SetActive(true);
    }

    public void InitializeStraight(Vector3 startPos, Vector3 aimAt)
    {
        isHoming = false;
        timer = 0f;
        transform.position = startPos;
        direction = ((Vector2)aimAt - (Vector2)startPos).normalized;
        AlignSpriteToDirection();
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
        if (isHoming && timer < homingDuration && target != null)
        {
            Vector2 desired = ((Vector2)target.position - (Vector2)transform.position).normalized;
            float angleDiff = Vector2.SignedAngle(direction, desired);
            float maxAngle = homingTurnSpeed * Time.deltaTime;
            float clamped = Mathf.Clamp(angleDiff, -maxAngle, maxAngle);
            float rad = clamped * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            Vector2 newDir = new Vector2(
                direction.x * cos - direction.y * sin,
                direction.x * sin + direction.y * cos
            ).normalized;
            direction = newDir;
            AlignSpriteToDirection();
        }
        else if (isHoming && timer >= homingDuration)
        {
            isHoming = false;
        }
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void AlignSpriteToDirection()
    {
        transform.right = -direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            pool.Release(this);
    }
}
