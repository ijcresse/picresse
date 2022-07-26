using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public List<List<bool>> puzzle;
    private List<HintLine> hintColumns;
    // private List<List<Hint>> hintColumns;
    private List<HintLine> hintRows;
    // private List<List<Hint>> hintRows;
    public GameObject hintLinePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreatePuzzle(int x, int y) {
        Debug.Log($"PuzzleController.CreatePuzzle called for puzzle with size {x} {y}");
        puzzle = new List<List<bool>>();
        for (int i = 0; i < y; i++) {
            string line = "";
            puzzle.Add(new List<bool>());
            for (int j = 0; j < x; j++) {
                puzzle[i].Add(Random.Range(0f, 1f) <= 0.6f);
                line += puzzle[i][j] ? 'o' : 'x';
            }
            Debug.Log(line);
        }

        Debug.Log($"created puzzle with {puzzle.Count} cols, {puzzle[0].Count} rows");
        CreateHints();
    }

    public void CreatePuzzle(string base64Code) {

    }

    List<bool> GetCol(int col) {
        List<bool> puzzleCol = new List<bool>();
        for (int i = 0; i < puzzle.Count; i++) {
            puzzleCol.Add(puzzle[i][col]);
        }
        return puzzleCol;
    }

    List<bool> GetRow(int row) {
        List<bool> puzzleRow = new List<bool>();
        for (int i = 0; i < puzzle[row].Count; i++) {
            puzzleRow.Add(puzzle[row][i]);
        }
        return puzzleRow;
    }

    private void CreateHints() {
        hintColumns = new List<HintLine>();
        hintRows = new List<HintLine>();
        Debug.Log("creating hints for coluimns");
        for (int x = 0; x < puzzle[0].Count; x++) {
            List<bool> col = GetCol(x);
            CreateHintLine(GetHints(col), true, x);
        }
        Debug.Log("now rows");
        for (int y = 0; y < puzzle.Count; y++) {
            List<bool> row = GetRow(y);
            CreateHintLine(GetHints(row), false, y);
        }
    }

    private void CreateHintLine(List<Hint> hints, bool isCol, int position) {
        GameObject hintBG = isCol ? GameObject.Find("HintColsBG") : GameObject.Find("HintRowsBG");
        float boxSize = GameObject.Find("Grid").GetComponent<GridController>().boxSize;
        GameObject hintLine = Instantiate(hintLinePrefab, hintBG.transform.position, hintLinePrefab.transform.rotation);
        Vector2 updatedPosition;
        Vector2 updatedScale;
        if (isCol) {
            updatedPosition = new Vector2(hintBG.transform.position.x + (position * boxSize) - (hintBG.transform.localScale.x / 2) + (boxSize / 2), 
                                          hintBG.transform.position.y);
            updatedScale = new Vector2(boxSize, hintBG.transform.localScale.y);
            hintLine.transform.position = updatedPosition;
            hintLine.transform.localScale = updatedScale;
        } else {
            updatedPosition = new Vector2(hintBG.transform.position.x, 
                                          hintBG.transform.position.y - (position * boxSize) + (hintBG.transform.localScale.y / 2) - (boxSize / 2));
            updatedScale = new Vector2(hintBG.transform.localScale.x, boxSize);
            hintLine.transform.position = updatedPosition;
            hintLine.transform.localScale = updatedScale;
        }
        hintLine.GetComponent<HintLine>().Init(hints, isCol, position, position == 0);
    }

    List<Hint> GetHints(List<bool> puzzleLine) {
        List<Hint> hints = new List<Hint>();
        int num = 0;
        string debug = "";
        for (int i = 0; i < puzzleLine.Count; i++) {
            if (puzzleLine[i]) {
                num++;
            } else {
                if (num > 0) {
                    hints.Add(new Hint(num, false));
                    debug = debug + ", " + num;
                    num = 0;
                }
            }
        }
        if (hints.Count == 0 || num > 0) {
            hints.Add(new Hint(num, false)); //bookends with last element, also covers 0 case
            debug = debug + ", " + num;
        }
        Debug.Log(debug);
        return hints;
    }

    public void CheckSolved(int[] grid, int row = -1, int col = -1) {
        // if (row != -1 && col != -1) {
        //     Debug.Log("ERROR PuzzleController.CheckSolved specifies both column and row");
        //     return;
        // } else if ((row < 0 && row > hints.GetLength(1)) || (col < 0 && col > hints.GetLength(0))) {
        //     Debug.Log($"ERROR PuzzleController.CheckSolved has illegal column ({col}) or row ({row}) length");
        // }

        // if (grid.Length < 0) {
        //     Debug.Log($"ERROR PuzzleController.CheckSolved grid parameter is invalid with length {grid.Length}");
        // }

        // bool[] hintArr;
        // if (col == -1) {
        //     hintArr = GetRow(row);
        // } else if (row == -1) {
        //     hintArr = GetCol(col);
        // } else {
        //     Debug.Log("ERROR PuzzleController.CheckSolved specifies neither column nor row");
        //     return;
        // }

        // int hintPtr = 0;
        // int gridPtr = 0;
        // int hintCounter = 0;
        // bool spaceReq = false;
        // bool error = false;
        // while (!error && gridPtr < grid.Length) {
        //     bool state = grid[gridPtr]; //state should be active / inactive / crossed
        //     if (state) {
        //         spaceReq = false;
        //         if (hintCounter > 0) {
        //             error = true;
        //         }
        //     } else {
        //         if (hintCounter == 0) {
        //             // hintCounter = hintArr[hintPtr]; //"safe check, check for hintCounter != 0 // huh? hintCounter is guaranteed to be 0 before this...
        //             hintPtr++;
        //         }
        //         hintCounter--;
        //         if (hintCounter < 0) {
        //             error = true;
        //         }
        //         if (hintCounter == 0) {
        //             spaceReq = true;
        //         }
        //     }
        //     gridPtr++;
        // }

        /*
        hintPtr, gridPtr = 0;
        spaceReq, error = false;
        hintCounter = 0;
        while (!error && gridPtr < grid.length)
            state = grid[gridPtr]
            if (state != active) {
                spaceReq = false
                if (hintCounter > 0) {
                    error = true;
                }
            } else if (state == active) {
                if (hintCounter == 0) {
                    hintCounter = hints[hintPtr] //safe check, check for hintCounter != 0)
                    hintPtr++;
                }
                hintCounter--;
                if hintCounter < 0 {
                    error = true;
                }
                if (hintCounter == 0) {
                    spaceReq = true;
                }
            }
            gridPtr++;
        */
    }

    public void CheckHints(int row, int col) {
        /*
        aPtr, bPtr, bRunner
        captureStart = B[0] == active
        while (bPtr < B.length)
            if captureStart
                bRunner = bPtr
                while
        */
    }
}
