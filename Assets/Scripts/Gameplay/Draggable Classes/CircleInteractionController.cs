using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D), typeof(Draggable))]
public class CircleInteractionController : MonoBehaviour
{
    [SerializeField] private CircleCollider2D interactionCircle;
    private BoxCollider2D boxCollider;
    private Draggable draggable;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        draggable = GetComponent<Draggable>();
    }

    private void Update()
    {
        if (EventSystem.current == null) return;

        bool isOverCircle = false;

        // Funciona para mouse y touch
        if (Input.touchCount > 0)
        {
            // Para móviles: usa el primer touch
            Touch touch = Input.GetTouch(0);
            isOverCircle = IsOverCircle(touch.position);
        }
        else
        {
            // Para editor/desktop: usa mouse
            isOverCircle = IsOverCircle(Input.mousePosition);
        }

        boxCollider.enabled = isOverCircle;
        draggable.enabled = isOverCircle;

        if (draggable.CanvasGroup != null)
        {
            draggable.CanvasGroup.blocksRaycasts = isOverCircle;
        }
    }

    private bool IsOverCircle(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return interactionCircle.OverlapPoint(worldPosition);
    }
}