using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Vector2 position = new Vector2(0, 0);
    private GridController gridScript;
    private float boxSize = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"starting position: ({position.x}, {position.y}");
        gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        boxSize = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        Debug.Log($"boxSize: {boxSize}");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 updatedPos = position + Vector2.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            updatedPos += Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            updatedPos += Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            updatedPos += Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            updatedPos += Vector2.down;
        }

        if (!updatedPos.Equals(position) && gridScript.CanMove(updatedPos))
        {
            position = updatedPos;
            //eventually this will be in constructor, as grid will need to know how big it is to scale down boxsize accordingly.
            Vector2 adjustedPos = new Vector2(position.x * boxSize, position.y * boxSize);
            Debug.Log($"attempting adjusted move to {adjustedPos.x}, {adjustedPos.y}");
            transform.position = adjustedPos;
        }
    }
}
