using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [Header("Pool Settings")]
    public Projectile projectilePrefab;
    public int defaultCapacity = 100;
    public int maxSize = 1000;

    private ObjectPool<Projectile> pool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { 
            Destroy(gameObject); 
            return; 
        }
        pool = new ObjectPool<Projectile>(
            createFunc: () => {
                Projectile proj = Instantiate(projectilePrefab);
                proj.pool = pool;
                return proj;
            },
            actionOnGet: proj => {
                proj.gameObject.SetActive(true);
            },
            actionOnRelease: proj => {
                proj.gameObject.SetActive(false);
            },
            actionOnDestroy: proj => {
                Destroy(proj.gameObject);
            },
            collectionCheck: true, // CHECK FOR MEMORY LEAKS, FALSE FOR PERFORMANCE (BUILDS)
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public Projectile Get() => pool.Get();

    public void Release(Projectile proj) => pool.Release(proj);
}
