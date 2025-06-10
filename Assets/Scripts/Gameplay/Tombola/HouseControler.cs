using System.Collections;
using UnityEngine;

public class HouseControler : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [Range(0.0f, 0.9f)] private float _insidePercentageThreshold = 0.000001f;
    private Collider2D _houseCollider;
    private bool _isDoorOpening = false;
    private bool _isPlayerInside = false;
    private Transform _playerTransform;
    private void Start()
    {
        _houseCollider = GetComponent<Collider2D>();
        if (!_houseCollider.isTrigger)
        {
            _houseCollider.isTrigger = true;
        }

        //if (_door != null)
        //   _door.SetActive(false);
    }

    public void OpenDoor() {
        StartCoroutine(OpenDoorTemporarily());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
        }
    }
    
     private void OnTriggerStay2D(Collider2D other)
     {
        if (other.CompareTag("Player") && !_isPlayerInside)
        {
            _isPlayerInside = IsPlayerMostlyInside(other);
        }
     }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _playerTransform = null;
        }
    }

    private IEnumerator CloseDoorTemporarily()
    {
        if(_isDoorOpening) yield break;

        _isDoorOpening = true;


        yield return new WaitForSeconds(0.2f);

        if (_isPlayerInside)
        {
            _door.SetActive(true);
            yield return new WaitForSeconds(5.0f);
            _door.SetActive(false);
        }

        _isDoorOpening = false;
    }

    private IEnumerator OpenDoorTemporarily()
    {
        yield return new WaitForSeconds(0.2f);
        _door.SetActive(false);
        yield return new WaitForSeconds(5.0f);
        // Before closing, check if player is partially inside
        //Debug.Log("Esta parcialmente dentro: " + IsPlayerMostlyInside(_playerTransform.GetComponent<Collider2D>()));
        if (_playerTransform != null && IsPlayerMostlyInside(_playerTransform.GetComponent<Collider2D>()))
        {
            _playerTransform.position = transform.position;
        }
        _door.SetActive(true);
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
        Debug.Log("Porcentaje dentro: " + (float)pointsInside / pointsToCheck.Length);
        Debug.Log("Porcentaje Threshold: " + _insidePercentageThreshold);
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
            Vector2 localPoint = transform.InverseTransformPoint(point);
            if (!_houseCollider.OverlapPoint(point))
            {
                return false;
            }
        }

        return true;
    }
}