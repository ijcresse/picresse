using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool[,] puzzle;
    private Hint[,] hints;
    private List<List<Hint>> hintColumns;
    private List<List<Hint>> hintRows;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreatePuzzle(int x, int y) {
        Debug.Log($"PuzzleController.CreatePuzzle called for puzzle with size {x},{y}");
        puzzle = new bool[y, x];
        for (int i = 0; i < y; i++) {
            string line = "";
            for (int j = 0; j < x; j++) {
                puzzle[i, j] = Random.Range(0f, 1f) <= 0.6f;
                line += puzzle[i, j] ? 'o' : 'x';
            }
            Debug.Log(line);
        }

        hints = new Hint[(int) Mathf.Ceil(y / 2), (int) Mathf.Ceil(x / 2)];
        CreateHints();
    }

    public void CreatePuzzle(string base64Code) {

    }

    bool[] GetRow(int row) {
        bool[] puzzleRow = new bool[puzzle.GetLength(0)];
        for (int i = 0; i < puzzle.GetLength(0); i++) {
            puzzleRow[i] = puzzle[row, i];
        }
        return puzzleRow;
    }

    bool[] GetCol(int col) {
        bool[] puzzleCol = new bool[puzzle.GetLength(1)];
        for (int i = 0; i < puzzle.GetLength(1); i++) {
            puzzleCol[i] = puzzle[i, col];
        }
        return puzzleCol;
    }

    
    private void CreateHints() {
        hintColumns = new List<List<Hint>>();
        hintRows = new List<List<Hint>>();
        Debug.Log("creating hints for coluimns");
        for (int y = 0; y < puzzle.GetLength(0); y++) {
            bool[] col = GetCol(y);
            hintColumns.Add(GetHints(col));
        }
        Debug.Log("now rows");
        for (int x = 0; x < puzzle.GetLength(1); x++) {
            bool[] row = GetRow(x);
            hintRows.Add(GetHints(row));
        }
    }

    List<Hint> GetHints(bool[] puzzleLine) {
        List<Hint> hints = new List<Hint>();
        int num = 0;
        string debug = "";
        for (int i = 0; i < puzzleLine.Length; i++) {
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
        if (num > 0) {
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

    class Hint {
        private int num {get; set;}
        private bool solved {get; set;}
        public Hint(int num, bool solved) {
            this.num = num;
            this.solved = solved;
        }
    }
}
