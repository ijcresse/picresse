using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int gameDimensionX;
    public int gameDimensionY;
    private GameObject gridSprite;
    private (float x, float y) gridSpriteSize;
    public float boxSize { get; set; }
    public Vector2 startPosition { get; set; }
    
    void Start()
    {
        gridSprite = GameObject.Find("GridBG");
        gridSpriteSize = (x: gridSprite.transform.localScale.x, y: gridSprite.transform.localScale.y);
        boxSize = gridSpriteSize.y / gameDimensionY; //always the smaller size. if we wanna do wacky stuff there'll have to be comparison
        Debug.Log($"boxSize: {boxSize}, gridSpriteSize: ({gridSpriteSize.x}, {gridSpriteSize.y})");
        startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2, 
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: -1);
        // }
        // else if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: 1);
        // }
        // else if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: 1);
        // }
        // else if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: -1);
        // }
    }
}
