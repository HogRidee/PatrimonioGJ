using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropCut : DragAndDrop
{
    protected override void SnapToCenter(RectTransform target)
    {
        transform.SetParent(target);
        RectTransform.anchoredPosition = new Vector2(-target.rect.width / 2, -target.rect.height / 2);
        RectTransform.sizeDelta = OriginalSize;
        transform.SetAsLastSibling();
    }
}
