using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool[,] puzzle;
    private Hint[,] hints;
    private GameObject[] hintColumns;
    private GameObject[] hintRows;

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
    }

    public void CreatePuzzle(string base64Code) {

    }

    bool[] GetRow(int row) {
        bool[] puzzleRow = new bool[puzzle.GetLength(0)];
        for (int i = 0; i < puzzle.GetLength(0); i++) {
            puzzleRow[i] = puzzle[i, row];
        }
        return puzzleRow;
    }

    bool[] GetCol(int col) {
        bool[] puzzleCol = new bool[puzzle.GetLength(1)];
        for (int i = 0; i < puzzle.GetLength(1); i++) {
            puzzleCol[i] = puzzle[col, i];
        }
        return puzzleCol;
    }

    List<Hint> GetHints(bool[] puzzleLine) {
        List<Hint> hints = new List<Hint>();
        int num = 0;
        for (int i = 0; i < puzzleLine.Length; i++) {
            if (puzzleLine[i]) {
                num++;
            } else if (!puzzleLine[i] && num < 0) {
                hints.Add(new Hint(num, false));
                num = 0;
            }
        }
        hints.Add(new Hint(num, false)); //bookends with last element, also covers 0 case
        return hints;
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
