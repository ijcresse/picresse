using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray = new Sprite[2];
    private int state;
    private const int INACTIVE = 0;
    private const int ACTIVE = 1;
    private const int CROSSED = 2;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        state = INACTIVE;
    }

    public int GetState() {
        return state;
    }

    public void SetState(int state) {
        if (state < INACTIVE || state > CROSSED) {
            Debug.Log($"BoxController.SetState(): Invalid state attempted: {state}");
            return;
        }
        spriteRenderer.sprite = spriteArray[state];
        this.state = state;
        Debug.Log($"Box.SetState: setting state to {state}");
    }
}
