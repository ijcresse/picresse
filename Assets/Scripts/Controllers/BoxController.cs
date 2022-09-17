using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray = new Sprite[2];
    private int state;

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
        this.state = state;
    }
}
