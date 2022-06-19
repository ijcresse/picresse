using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private const int TOP_LAYER = 3;
    private (float x, float y) HOME_POSITION = (-1f, 1.6f);
    private (float x, float y) position;
    private GridController gridScript;
    private float boxSize = 0f;

    void Start()
    {
        gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = TOP_LAYER;
        boxSize = spriteRenderer.sprite.bounds.size.x;

        position.x = this.gameObject.transform.position.x;
        position.y = this.gameObject.transform.position.y;
        Debug.Log($"starting position: ({position.x}, {position.y})");
        Debug.Log($"boxSize: {boxSize}");
    }

    void Update()
    {
        (float x, float y) updatedPos = position;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            updatedPos.x = position.x - boxSize;
            Debug.Log($"moved left: {updatedPos.x}");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            updatedPos.x = position.x + boxSize;
            Debug.Log($"moved right: {updatedPos.x}");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            updatedPos.y = position.y + boxSize;
            Debug.Log($"moved up: {updatedPos.y}");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            updatedPos.y = position.y - boxSize;
            Debug.Log($"moved down: {updatedPos.y}");
        }

        // if (!updatedPos.Equals(position) && gridScript.CanMove(updatedPos))
        if (!updatedPos.Equals(position))
        {
            position = updatedPos;
            //eventually this will be in constructor, as grid will need to know how big it is to scale down boxsize accordingly.
            Vector2 adjustedPos = new Vector2(updatedPos.x, updatedPos.y);
            Debug.Log($"attempting adjusted move to {adjustedPos.x}, {adjustedPos.y}");
            transform.position = adjustedPos;
        }
    }
}
