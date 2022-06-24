using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject clock;
    public GameObject hintCols;
    public GameObject hintRows;
    private GridController gridScript;
    public GameObject cursorPrefab;
    private CursorController cursorScript;
    
    void Start()
    {
        gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        float boxSize = gridScript.boxSize;

        GameObject cursor = Instantiate(cursorPrefab, gridScript.startPosition, cursorPrefab.transform.rotation);
        cursor.transform.localScale = new Vector2(boxSize * 2, boxSize * 2);
        cursorScript = cursor.GetComponent<CursorController>();
    }

    void Update()
    {
        int gameDimensionX = gridScript.gameDimensionX;
        int gameDimensionY = gridScript.gameDimensionY;
        float boxSize = gridScript.boxSize;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: -1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: 1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: -1);
        }
    }
}
