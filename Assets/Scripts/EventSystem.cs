using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public event Action<int, int> onCursorMovedTo;
    public event Action<int, int, int, List<List<int>>> onBoxUpdated;
    public event Action onAlertWin;

    private void Awake()
    {
        current = this;
    }

    public void CursorMovedTo(int column, int row) {
        if (onCursorMovedTo != null) {
            onCursorMovedTo(column, row);
        }
    }

    public void BoxUpdated(int stateUpdate, int col, int row, List<List<int>> colThenRow)
    {
        if (onBoxUpdated != null)
        {
            onBoxUpdated(stateUpdate, col, row, colThenRow);
        }
    }

    public void AlertWin()
    {
        if (onAlertWin != null)
        {
            onAlertWin();
        }
    }
}
