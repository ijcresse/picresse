using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintLine : MonoBehaviour
{
    public List<Hint> hints {get; private set;}
    bool isCol;
    public bool solved {get; set;}
    int position; //leftmost/topmost is 0
    public GameObject hintTextPrefab;
    private TextMeshProUGUI hintText;
    public SpriteRenderer hintLineSprite;

    public void Init(List<Hint> hints, bool isCol, int position, bool active, int lineSize) {
        this.hints = hints;
        CreateHintRanges(lineSize);
        this.isCol = isCol;
        this.position = position;
        this.solved = hints[0].num == 0; //if hint is 0, no more hints should exist.
        
        hintText = Instantiate(hintTextPrefab, new Vector2(0f, 0f), hintTextPrefab.transform.rotation).GetComponent<TextMeshProUGUI>();
        if (isCol)
        {
            hintText.transform.SetParent(GameObject.Find("HintColsUI").transform, false);
        } else
        {
            hintText.transform.SetParent(GameObject.Find("HintRowsUI").transform, false);
        }

        SetText();

        EventSystem.current.onCursorMovedTo += OnActivated;

        hintLineSprite = gameObject.GetComponent<SpriteRenderer>();
        Color color = hintLineSprite.color;
        color.a = active ? 100 : 0;
        hintLineSprite.color = color;
    }

    private void CreateHintRanges(int size)
    {
        int earliestPos = 0;
        for (int i = 0; i < hints.Count; i++)
        {
            hints[i].earliestPosition = earliestPos;
            earliestPos += hints[i].num + 1;
        }
        int latestPos = size - 1; //adjust for 0 based index
        for (int i = hints.Count - 1; i >= 0; i--)
        {
            hints[i].latestPosition = latestPos;
            latestPos -= (hints[i].num + 1);
        }
    }

    public bool CalculateSolved(List<int> gridLine) {
        int gridPtr = 0;
        for (int i = 0; i < hints.Count; i++)
        {
            Hint currentHint = hints[i];
            
            //find first new instance of active
            while (gridPtr < gridLine.Count && gridLine[gridPtr] != Constants.ACTIVE)
            {
                gridPtr++;
            }

            //check to make sure hint is contiguously solved
            for (int j = 0; j < currentHint.num; j++)
            {
                if (gridPtr >= gridLine.Count || gridLine[gridPtr] != Constants.ACTIVE)
                {
                    currentHint.solved = false;
                    return false;
                }
                gridPtr++;
            }
            currentHint.solved = true;

            //check for one active too many
            if (gridPtr < gridLine.Count && i < hints.Count - 1 && gridLine[gridPtr] == Constants.ACTIVE)
            {
                return false;
            } else if (gridPtr >= gridLine.Count && i < hints.Count - 1)
            {
                return false;
            }
        }

        //hints all solved. go through and make sure there are no additional active boxes.
        for (; gridPtr < gridLine.Count; gridPtr++)
        {
            if (gridLine[gridPtr] == Constants.ACTIVE)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckCaptures(List<int> gridLine)
    {
        ClearHints();
        int gridPtr = 0;
        bool capturing = true;
        int captureCount = 0;
        while(gridPtr < gridLine.Count)
        {
            if (gridLine[gridPtr] == Constants.ACTIVE && capturing)
            {
                captureCount++;
                if (gridPtr == gridLine.Count - 1) //wall check
                {
                    FindValidHintInRange(gridPtr, captureCount);
                }
            } else if (gridLine[gridPtr] == Constants.INACTIVE && capturing)
            {
                captureCount = 0;
                capturing = false;
            } else if (gridLine[gridPtr] == Constants.CROSSED || gridPtr == gridLine.Count - 1) //latter accounts for end wall
            {
                if (capturing)
                {
                    int adjustedPos = gridPtr == gridLine.Count - 1 ? gridPtr : gridPtr - 1; //check one step back from cross
                    FindValidHintInRange(adjustedPos, captureCount);
                } else
                {
                    capturing = true;
                }
                captureCount = 0;
            }
            gridPtr++;
        }
        SetText();
    }

    private void ClearHints()
    {
        for (int i = 0; i < hints.Count; i++)
        {
            hints[i].solved = false;
        }
    }
    private void FindValidHintInRange(int gridPtr, int captureCount)
    {
        int match = -1;
        for (int i = 0; i < hints.Count; i++)
        {
            if (hints[i].earliestPosition <= gridPtr && gridPtr <= hints[i].latestPosition &&
                hints[i].num == captureCount && !hints[i].solved)
            {
                if (match != -1)
                {
                    return;
                } else
                {
                    match = i;
                }
            }
        }
        if (match != -1) //in case no match was found
        {
            hints[match].solved = true;
        }
    }

    //TODO: SCALE TEXT TO ROW SIZE(?)
    //TODO: THESE COLOR VALS SHOULD COME FRO MA COLOR OBJ
    private void SetText() {
        string text = "";
        for (int i = 0; i < hints.Count; i++)
        {
            if (isCol)
            {
                if (solved || hints[i].solved)
                {
                    text = text + "<color=#848C6C>" + hints[i].num + "</color>\n";
                } else
                {
                    text = text + "<color=#1d2912>" + hints[i].num + "</color>\n";
                }
            }
            else
            {
                if (solved || hints[i].solved)
                {
                    text = text + " <color=#848C6C>" + hints[i].num + "</color>";
                } else
                {
                    text = text + " <color=#1d2912>" + hints[i].num + "</color>";
                }
            }
        }
        hintText.text = text;
    }

    private void OnActivated(int column, int row) { //ok one, we're having a problem with how the system adds/removes too much alpha. two, it says it's deactivating, but it's not?
        Color color = hintLineSprite.color;
        if (isCol && column == position) {
            color.a = 100;
        } else if (!isCol && row == position) {
            color.a = 100;
        } else {
            color.a = 0;
        }

        if (hintLineSprite.color.a != color.a) { //update only when we detect an update!
            hintLineSprite.color = color;
        }
    }

    private void OnDestroy() {
        EventSystem.current.onCursorMovedTo -= OnActivated;
    }
}
