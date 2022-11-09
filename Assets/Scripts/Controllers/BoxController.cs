using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray = new Sprite[2];
    private int state;
    private string color = "#9A3013";

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        state = Constants.INACTIVE;
    }

    public int GetState() {
        return state;
    }

    public void SetState(int state) {
        if (state < Constants.INACTIVE || state > Constants.CROSSED) {
            Debug.Log($"BoxController.SetState(): Invalid state attempted: {state}");
            return;
        }
        spriteRenderer.sprite = spriteArray[state];
        //TODO: THESE SHOULD COME FROM A COLOR OBJ
        if (state > 0)
        {
            spriteRenderer.color = new Color(154f / 255f, 47f / 255f, 18f / 255f);
        } else
        {
            spriteRenderer.color = new Color(236f / 255f, 228f / 255f, 215f / 255f);
        }
        this.state = state;
    }
}
