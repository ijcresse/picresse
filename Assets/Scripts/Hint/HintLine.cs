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
    private SpriteRenderer hintLineSprite;
    
    public void Init(List<Hint> hints, bool isCol, int position, bool active) {
        this.hints = hints;
        this.isCol = isCol;
        this.position = position;
        this.active = active;
        this.solved = false;
        
        hintText = Instantiate(hintTextPrefab, new Vector2(0f, 0f), hintTextPrefab.transform.rotation).GetComponent<TextMeshProUGUI>();
        hintText.transform.SetParent(GameObject.Find("Canvas").transform, false);
        
        string text = "";
        for(int i = 0; i < hints.Count ; i++) {
            if (isCol) {
                text = text + hints[i].num + "\n";
            } else {
                text = text + " " + hints[i].num;
            }
        }
        hintText.text = text;

        EventSystem.current.onCursorMovedTo += OnActivated;
    }

    private void CalculateSolved() {

    }

    private void SetText() {
        
    }

    private void OnActivated(int x, int y) {
        // Debug.Log($"HintLine: moving to {x}, {y}");
        if (isCol && y == position) {
            //activate
        } else if (!isCol && x == position) {
            //activate
        } else {
            //deactivate
        }
    }

    private void OnDestroy() {
        EventSystem.current.onCursorMovedTo -= OnActivated;
    }
}
