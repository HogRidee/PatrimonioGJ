using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Vector2 originalSize;
    private bool isDragging = false;
    private bool isLocked = false;
    private float originalAlpha;
    private Vector3 originalScale;
    public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }
    public Canvas Canvas { get => canvas; set => canvas = value; }
    public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
    public Vector2 OriginalPosition { get => originalPosition; set => originalPosition = value; }
    public Transform OriginalParent { get => originalParent; set => originalParent = value; }
    public Vector2 OriginalSize { get => originalSize; set => originalSize = value; }
    public bool IsDragging { get => isDragging; set => isDragging = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public Vector3 OriginalScale { get => originalScale; set => originalScale = value; }

    

    void Update()
    {
        //Debug.Log($"BlocksRaycasts: {canvasGroup.blocksRaycasts}, isLocked: {isLocked}");
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSize = rectTransform.sizeDelta;
        OriginalScale = rectTransform.localScale;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalAlpha = canvasGroup.alpha;

        DragManager.Instance.RegisterDraggable(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;

        isDragging = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked || !isDragging) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Elimina la verificación de isLocked aquí
        isDragging = false;
        canvasGroup.alpha = originalAlpha;
        StartCoroutine(ForceUpdate());
    }

    private IEnumerator ForceUpdate()
    {
        yield return null;
        if (!isDragging && !isLocked)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Lock()
    {
        isLocked = true;
        canvasGroup.alpha = 0.4f; // Más opaco para indicar que está bloqueado
        canvasGroup.blocksRaycasts = false;
    }

    public void Unlock()
    {
        isLocked = false;
        canvasGroup.alpha = originalAlpha;
        canvasGroup.blocksRaycasts = true;
    }

    private void OnDestroy()
    {
        // Desregistra el objeto cuando es destruido
        if (DragManager.Instance != null)
        {
            DragManager.Instance.UnregisterDraggable(this);
        }
    }
}