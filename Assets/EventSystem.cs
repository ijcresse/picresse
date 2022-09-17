using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public event Action<int, int> onCursorMovedTo;
    public event Action<int, int, int, List<List<int>>> onBoxUpdated;

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

    public void BoxUpdated(int stateUpdate, int col, int row, List<List<int>> colThenRow)
    {
        if (onBoxUpdated != null)
        {
            onBoxUpdated(stateUpdate, col, row, colThenRow);
        } else
        {
            Debug.Log("ERROR EventSystem.BoxUpdated: no registered listeners");
        }
    }
}
