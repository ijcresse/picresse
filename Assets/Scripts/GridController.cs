using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    private (float x, float y) gameDimensions;
    private GameObject gridSprite;
    private (float x, float y) gridSpriteSize;
    public float boxSize { get; set; }

    public GameObject cursorPrefab;
    private CursorController cursorScript;
    
    void Start()
    {
        gameDimensions = (x: 5, y: 5);
        gridSprite = GameObject.Find("GridBG");
        gridSpriteSize = (x: gridSprite.transform.localScale.x, y: gridSprite.transform.localScale.y);
        boxSize = gridSpriteSize.y / gameDimensions.y;
        Debug.Log($"boxSize: {boxSize}, gridSpriteSize: ({gridSpriteSize.x}, {gridSpriteSize.y})");

        Vector2 startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2, 
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);

        GameObject cursor = Instantiate(cursorPrefab, startPosition, cursorPrefab.transform.rotation);
        cursor.transform.localScale = new Vector2(boxSize * 2, boxSize * 2);
        cursorScript = cursor.GetComponent<CursorController>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorScript.Move(gameDimensions.x, gameDimensions.y, boxSize, xMove: -1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorScript.Move(gameDimensions.x, gameDimensions.y, boxSize, xMove: 1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cursorScript.Move(gameDimensions.x, gameDimensions.y, boxSize, yMove: 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursorScript.Move(gameDimensions.x, gameDimensions.y, boxSize, yMove: -1);
        }
    }
}
