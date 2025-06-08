// Projectile.cs (versión rectilínea)
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public IObjectPool<Projectile> pool;
    public float speed = 20f;
    public float lifeTime = 2f;

    private Vector3 direction;
    private float timer;

    public void Initialize(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        direction = (targetPosition - startPosition).normalized;
        timer = 0f;
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
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // CAN ADD TAGS
        pool.Release(this);
    }
}
