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
        
        
        string text = "";
        for(int i = 0; i < hints.Count ; i++) {
            if (isCol) {
                text = text + hints[i].num + "\n";
            } else {
                text = text + " " + hints[i].num;
            }
        }
        hintText.text = text;
        /*
        float boxSize = GameObject.Find("Grid").GetComponent<GridController>().boxSize;
        Vector3 gamePos = isCol ?
                            new Vector3(gameObject.transform.position.x + (boxSize / 2), gameObject.transform.position.y, 0f) :
                            new Vector3(gameObject.transform.position.x - (boxSize / 2), gameObject.transform.position.y + (boxSize / 2), 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(gamePos);
        Vector3 adjustedPos = new Vector3 (screenPos.x - (Screen.width / 2), screenPos.y - (Screen.height / 2), 0f);
        hintText.GetComponent<RectTransform>().anchoredPosition3D = adjustedPos;
        */
        EventSystem.current.onCursorMovedTo += OnActivated;

        hintLineSprite = gameObject.GetComponent<SpriteRenderer>();
        Color color = hintLineSprite.color;
        color.a = active ? 100 : 0;
        hintLineSprite.color = color;
    }

    //works in happy case, but fails on at least too many dots. (eg hint 1 on 2x2, with 2 dots filled)
    public bool CalculateSolved(List<int> gridLine) {
        int gridPtr = 0;
        for (int i = 0; i < hints.Count; i++)
        {
            Hint currentHint = hints[i];
            
            //find first new instance of active

            //ok, this is going out of rangeof gridLine. also it's not stopping on the first instance. whoops! figure out the boolean condition to exit out properly!
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
                gridPtr++;
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
        for (; gridPtr < gridLine.Count; gridPtr++)
        {
            if (gridLine[gridPtr] == Constants.ACTIVE)
            {
                return false;
            }
        }
        Debug.Log($"DEBUG HintLine.CalculateSolved: isCol {isCol}, position {position} is now solved. ");
        return true;
    }

    private void SetText() {
        
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
