using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    private static DragManager instance;
    private List<Draggable> draggables = new List<Draggable>();

    public static DragManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DragManager>();
                if (instance == null)
                    Debug.Log("No existe Drag Manager");
            }
            return instance;
        }
    }

    public void RegisterDraggable(Draggable draggable)
    {
        if (!draggables.Contains(draggable))
        {
            draggables.Add(draggable);
        }
    }

    public void UnregisterDraggable(Draggable draggable)
    {
        if (draggables.Contains(draggable))
        {
            draggables.Remove(draggable);
        }
    }

    public void LockAllDraggables()
    {
        foreach (Draggable draggable in draggables)
        {
            draggable.Lock();
        }
    }

    public void LockADraggableObject(Draggable draggable)
    {
        draggable.Lock();
    }
    public void UnlockAllDraggables()
    {
        foreach (Draggable draggable in draggables)
        {
            draggable.Unlock();
        }
    }

    public void UnlockADraggableObject(Draggable draggable)
    {
        draggable.Unlock();
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

