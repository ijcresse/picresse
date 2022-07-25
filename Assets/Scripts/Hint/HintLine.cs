using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintLine : MonoBehaviour
{
    public List<Hint> hints {get;}
    bool isCol;
    bool active {get; set;} //true when cursor is on this line
    public bool solved {get; set;}
    Vector2 position;
    private TextMeshProUGUI hintText;
    private SpriteRenderer hintLineSprite;

    void Start() {
        //instantiate hinttext
    }
    
    public HintLine(List<Hint> hints, bool isCol, bool active, bool solved, Vector2 position, float boxSize) {
        this.hints = hints;
        this.isCol = isCol;
        this.active = active;
        this.solved = solved;
        this.position = position;

        //instantiate a hintline object from a prefab and manage its sprite width from boxsize
        // hintLineSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void CalculateSolved() {

    }

    private void SetText() {
        
    }

    // public void SetActive(bool active) {
    //     this.active = active;
    //     if (active) {
    //         hintLineSprite.color.a += 100;
    //     } else {
    //         hintLineSprite.color.a -= 100;
    //     }
    // }
}
