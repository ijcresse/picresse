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

        float boxSize = GameObject.Find("Grid").GetComponent<GridController>().boxSize;
        Vector3 gamePos = isCol ?
                            new Vector3(gameObject.transform.position.x + (boxSize / 2), gameObject.transform.position.y, 0f) :
                            new Vector3(gameObject.transform.position.x - (boxSize / 2), gameObject.transform.position.y + (boxSize / 2), 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(gamePos);
        Vector3 adjustedPos = new Vector3 (screenPos.x - (Screen.width / 2), screenPos.y - (Screen.height / 2), 0f);
        hintText.GetComponent<RectTransform>().anchoredPosition3D = adjustedPos;
        EventSystem.current.onCursorMovedTo += OnActivated;

        hintLineSprite = gameObject.GetComponent<SpriteRenderer>();
        Color color = hintLineSprite.color;
        color.a = active ? 100 : 0;
        hintLineSprite.color = color;
    }

    private void CalculateSolved() {

    }

    private void SetText() {
        
    }

    private void OnActivated(int column, int row) { //ok one, we're having a problem with how the system adds/removes too much alpha. two, it says it's deactivating, but it's not?
        Color color = hintLineSprite.color;
        if (isCol && column == position) {
            active = true;
            color.a = 100;
            Debug.Log($"HintLine: OnActivated. activating column {column}. color.a {color.a}");
        } else if (!isCol && row == position) {
            active = true;
            color.a = 100;
            Debug.Log($"HintLine: OnActivated. activating row {row}. color.a {color.a}");
        } else {
            active = false;
            color.a = 0;
            Debug.Log($"HintLine: OnActivated. deactivating. color.a {color.a}");
        }

        if (hintLineSprite.color.a != color.a) { //update only when we detect an update!
            hintLineSprite.color = color;
        }
    }

    private void OnDestroy() {
        EventSystem.current.onCursorMovedTo -= OnActivated;
    }
}
