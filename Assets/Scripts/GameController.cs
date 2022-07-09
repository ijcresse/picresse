using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int gameDimensionX;
    public int gameDimensionY;
    public GameObject clock;
    public GameObject hintCols;
    public GameObject hintRows;
    private GridController gridScript;
    public GameObject cursorPrefab;
    private CursorController cursorScript;
    private PuzzleController puzzleScript;
    
    void Start()
    {
        gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        gridScript.gameDimensionX = gameDimensionX;
        gridScript.gameDimensionY = gameDimensionY;
        gridScript.SetUpGrid();
        
        float boxSize = gridScript.boxSize;

        GameObject cursor = Instantiate(cursorPrefab, gridScript.startPosition, cursorPrefab.transform.rotation);
        cursor.transform.localScale = new Vector2(boxSize * 2, boxSize * 2);
        cursorScript = cursor.GetComponent<CursorController>();

        puzzleScript = GameObject.Find("Puzzle").GetComponent<PuzzleController>();
        puzzleScript.CreatePuzzle(gameDimensionX, gameDimensionY);
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
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: -1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: 1);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            (int x, int y) = cursorScript.GetGamePosition();
            gridScript.SetCellState(x, y, true);
        } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
            (int x, int y) = cursorScript.GetGamePosition();
            gridScript.SetCellState(x, y, false);
        }
    }
}
