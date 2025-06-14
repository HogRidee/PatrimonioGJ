using System.Collections;
using UnityEngine;

public class HouseControler : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [Range(0.0f, 0.9f)] private float _insidePercentageThreshold = 0.000001f;
    private Collider2D _houseCollider;
    private bool _isPlayerInside = false;
    private Transform _playerTransform;
    private Player_Movement _player = null;
    private Coroutine _closeDoorCoroutine;

    private void Start()
    {
        _houseCollider = GetComponent<Collider2D>();
        if (!_houseCollider.isTrigger)
        {
            _houseCollider.isTrigger = true;
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorTemporarily());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
            _player = other.GetComponent<Player_Movement>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isPlayerInside)
        {
            _isPlayerInside = IsPlayerMostlyInside(other);
        }

        if (other.GetComponent<Projectile>() != null)
        {
            if (_playerTransform != null && IsPlayerFullyInside(_playerTransform.GetComponent<Collider2D>()))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _playerTransform = null;

            if (IsDoorOpen())
            {
                if (_closeDoorCoroutine != null)
                    StopCoroutine(_closeDoorCoroutine);

                _closeDoorCoroutine = StartCoroutine(CloseDoorWithDelay(0.3f));
            }
        }
    }

    private IEnumerator OpenDoorTemporarily()
    {
        yield return new WaitForSeconds(0.2f);
        _door.SetActive(false);

        yield return new WaitForSeconds(4.8f);

        var playerTransform = _playerTransform;
        var player = _player;

        if (playerTransform == null || player == null)
        {
            _door.SetActive(true);
            yield break;
        }

        if (IsPlayerMostlyInside(playerTransform.GetComponent<Collider2D>()))
        {
            playerTransform.position = transform.position;
        }

        yield return new WaitForSeconds(0.2f);

        if (playerTransform != null && IsPlayerFullyInside(playerTransform.GetComponent<Collider2D>()))
        {
            if(!player.HasPowerUp)
                player.MakePowerfull();

            while (playerTransform != null)
            {
                if (!IsPlayerFullyInside(playerTransform.GetComponent<Collider2D>()))
                    break;

                if (player.GetHealth() <= 0)
                    break;

                yield return new WaitForSeconds(2.0f);

                if (!IsPlayerFullyInside(playerTransform.GetComponent<Collider2D>()))
                    break;

                Debug.Log("Penalización por quedarse en casa");
                player.TakeDamage(1);
            }
        }
        while (playerTransform != null)
        {
            if (player.GetHealth() <= 0)
                break;

            if (!IsPlayerFullyInside(playerTransform.GetComponent<Collider2D>()))
                break;

            yield return new WaitForSeconds(0.5f);
        }

        _door.SetActive(true);
    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_playerTransform == null || !IsPlayerFullyInside(_playerTransform.GetComponent<Collider2D>()))
        {
            _door.SetActive(true);
        }
    }

    private bool IsPlayerMostlyInside(Collider2D playerCollider)
    {
        var playerBounds = playerCollider.bounds;
        Vector2[] pointsToCheck = new Vector2[]
        {
            playerBounds.center,
            playerBounds.min,
            playerBounds.max,
            new Vector2(playerBounds.min.x, playerBounds.max.y),
            new Vector2(playerBounds.max.x, playerBounds.min.y),
            playerCollider.transform.position
        };

        int pointsInside = 0;

        foreach (Vector2 point in pointsToCheck)
        {
            if (_houseCollider.OverlapPoint(point))
            {
                pointsInside++;
            }
        }

        return (float)pointsInside / pointsToCheck.Length >= _insidePercentageThreshold;
    }

    private bool IsPlayerFullyInside(Collider2D playerCollider)
    {
        var playerBounds = playerCollider.bounds;
        Vector2[] pointsToCheck = new Vector2[]
        {
            playerBounds.center,
            playerBounds.min,
            playerBounds.max,
            new Vector2(playerBounds.min.x, playerBounds.max.y),
            new Vector2(playerBounds.max.x, playerBounds.min.y),
            playerCollider.transform.position
        };

        foreach (Vector2 point in pointsToCheck)
        {
            if (!_houseCollider.OverlapPoint(point))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsDoorOpen()
    {
        return !_door.activeSelf;
    }
}
