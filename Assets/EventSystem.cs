using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public event Action<int, int> onCursorMovedTo;

    private void Awake()
    {
        current = this;
    }

    public void CursorMovedTo(int column, int row) {
        if (onCursorMovedTo != null) {
            onCursorMovedTo(column, row);
        } else {
            Debug.Log("ERROR EventSystem.CursorMovedTo: no registered listeners");
        }
    }
}
