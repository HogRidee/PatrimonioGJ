using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : Draggable
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (IsLocked) return;

        IsDragging = false;
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;

        GameObject dropZone = eventData.pointerCurrentRaycast.gameObject;

        if (dropZone != null)
        {
            if (dropZone.CompareTag("DropValid"))
            {
                SnapToCenter(dropZone.GetComponent<RectTransform>());
                DragManager.Instance.LockAllDraggables();
                DragManager.Instance.UnlockADraggableObject(this);
            }
            else if (dropZone.CompareTag("DropInvalid"))
            {
                ReturnToOriginalPosition();
                DragManager.Instance.UnlockAllDraggables();
            }
        }
        else 
        {
            ReturnToOriginalPosition();
            DragManager.Instance.UnlockAllDraggables();
        }
    }

    protected virtual void SnapToCenter(RectTransform target)
    {
        transform.SetParent(target);
        RectTransform.anchoredPosition = Vector2.zero;
        RectTransform.sizeDelta = OriginalSize;
        transform.SetAsLastSibling();
        RectTransform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(OriginalParent);
        RectTransform.anchoredPosition = OriginalPosition;
        RectTransform.sizeDelta = OriginalSize;
        RectTransform.localScale = OriginalScale;
    }
}