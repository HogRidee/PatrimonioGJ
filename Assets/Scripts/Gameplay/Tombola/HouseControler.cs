using System.Collections;
using UnityEngine;

public class HouseControler : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    private Collider2D _houseCollider;
    private bool _isDoorOpening = false;
    private bool _isPlayerInside = false;

    private void Start()
    {
        _houseCollider = GetComponent<Collider2D>();
        if (!_houseCollider.isTrigger)
        {
            _houseCollider.isTrigger = true;
        }

        if (_door != null)
            _door.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isPlayerInside && !_isDoorOpening)
        {
            if (IsPlayerFullyInside(other))
            {
                _isPlayerInside = true;
                StartCoroutine(CloseDoorTemporarily());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
        }
    }

    private IEnumerator CloseDoorTemporarily()
    {
        if (_isDoorOpening) yield break;

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