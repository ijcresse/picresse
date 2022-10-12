using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintLine : MonoBehaviour
{
    public List<Hint> hints {get; private set;}
    bool isCol;
    bool active {get; set;} //true when cursor is on this line
    public bool solved {get; private set;}
    int position; //leftmost/topmost is 0
    public GameObject hintTextPrefab;
    private TextMeshProUGUI hintText;
    public SpriteRenderer hintLineSprite;

    public void Init(List<Hint> hints, bool isCol, int position, bool active) {
        this.hints = hints;
        this.isCol = isCol;
        this.position = position;
        this.active = active;
        this.solved = false;
        
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

    //works in happy case, but fails on at least too many dots. (eg hint 1 on 2x2, with 2 dots filled)
    public bool CalculateSolved(List<int> gridLine) {
        solved = false;
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

            if (gridPtr < gridLine.Count && i < hints.Count - 1)
            {
                if (gridLine[gridPtr] == Constants.ACTIVE)
                {
                    return false;
                }
            } else if (gridPtr >= gridLine.Count && i < hints.Count - 1)
            {
                return false;
            }
        }

        //hints all solved. go through and make sure there are no additional active boxes.
        gridPtr++;
        for (; gridPtr < gridLine.Count; gridPtr++)
        {
            if (gridLine[gridPtr] == Constants.ACTIVE)
            {
                return false;
            }
        }
        solved = true;
        return true;
    }

    public void CheckCaptures(List<int> gridLine)
    {
        ClearHints();
        int gridPtr = 0;
        bool capturing = true; //wall counts as capturer //still not finding the wall capture unfortuantely!
        int captureCount = 0;
        bool captureFound = false;
        while(gridPtr < gridLine.Count)
        {
            if (gridLine[gridPtr] == Constants.ACTIVE && capturing)
            {
                captureCount++;
            }
            if (gridLine[gridPtr] == Constants.INACTIVE && capturing)
            {
                captureCount = 0;
                capturing = false;
            }
            if (gridLine[gridPtr] == Constants.CROSSED || gridPtr == gridLine.Count - 1) //latter accounts for end wall
            {
                if (capturing) //end current capture
                {
                    Dictionary<int, List<int>> hintsInRange = FindValidHintsInRange(gridPtr);
                    if (hintsInRange.ContainsKey(captureCount))
                    {
                        if (hintsInRange[captureCount].Count == 1) //unique value found, can secure capture
                        {
                            int capturedHintPosition = hintsInRange[captureCount][0];
                            hints[capturedHintPosition].solved = true;
                            captureFound = true;
                        }
                    } else
                    {
                        //this capture doesn't match any valid hints, we're in error
                    }
                } else
                {
                    capturing = true;
                    captureCount = 0;
                }
            }
            gridPtr++;
        }

        if (captureFound)
        {
            SetText();
        }
    }

    private void ClearHints()
    {
        for (int i = 0; i < hints.Count; i++)
        {
            hints[i].solved = false;
        }
    }

    private Dictionary<int, List<int>> FindValidHintsInRange(int position)
    {
        Dictionary<int, List<int>> hintsInRange = new Dictionary<int, List<int>>();
        int cumulativeHintValue = 0;
        for (int i = 0; i < hints.Count; i++)
        {
            cumulativeHintValue += hints[i].num;
            if (position >= cumulativeHintValue + i) //value of i is number of spaces required
            {
                if (hintsInRange.ContainsKey(hints[i].num))
                {
                    hintsInRange[hints[i].num].Add(i); //storing position
                } else
                {
                    hintsInRange[hints[i].num] = new List<int> { i }; 
                }
            }
        }
        return hintsInRange;
    }

    private void SetText() { //TODO: this is buggy. only fills one star, doesn't clear stars. fix it!!!
        string text = "";
        for (int i = 0; i < hints.Count; i++)
        {
            if (isCol)
            {
                if (hints[i].solved)
                {
                    Debug.Log("hint solved, marking");
                    text = text + "*" + hints[i].num + "\n"; ;
                } else
                {
                    text = text + hints[i].num + "\n";
                }
            }
            else
            {
                if (hints[i].solved)
                {
                    Debug.Log("hint solved, marking");
                    text = text + "*" + hints[i].num;
                } else
                {
                    text = text + " " + hints[i].num;
                }
            }
        }
        hintText.text = text;
    }

    private void OnActivated(int column, int row) { //ok one, we're having a problem with how the system adds/removes too much alpha. two, it says it's deactivating, but it's not?
        Color color = hintLineSprite.color;
        if (isCol && column == position) {
            active = true;
            color.a = 100;
        } else if (!isCol && row == position) {
            active = true;
            color.a = 100;
        } else {
            active = false;
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
