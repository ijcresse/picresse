using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public List<List<bool>> puzzle;
    private List<HintLine> hintColumns;
    private List<HintLine> hintRows;
    public GameObject hintLinePrefab;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.onBoxUpdated += OnBoxUpdated;
    }

    public void CreatePuzzle(int x, int y, Vector2 startPosition) {
        puzzle = new List<List<bool>>();
        for (int i = 0; i < y; i++) {
            string line = "";
            puzzle.Add(new List<bool>());
            for (int j = 0; j < x; j++) {
                puzzle[i].Add(Random.Range(0f, 1f) <= 0.6f);
                line += puzzle[i][j] ? 'o' : 'x';
            }
        }
        CreateHints(startPosition);
    }

    public void CreatePuzzle(string base64Code) {

    }

    List<bool> GetCol(int col) {
        List<bool> puzzleCol = new();
        for (int i = 0; i < puzzle.Count; i++) {
            puzzleCol.Add(puzzle[i][col]);
        }
        return puzzleCol;
    }

    List<bool> GetRow(int row) {
        List<bool> puzzleRow = new();
        for (int i = 0; i < puzzle[row].Count; i++) {
            puzzleRow.Add(puzzle[row][i]);
        }
        return puzzleRow;
    }

    private void CreateHints(Vector2 startPosition) {
        hintColumns = new List<HintLine>();
        hintRows = new List<HintLine>();
        for (int x = 0; x < puzzle[0].Count; x++) {
            List<bool> col = GetCol(x);
            CreateHintLine(GetHints(col), true, x, startPosition);
        }
        for (int y = 0; y < puzzle.Count; y++) {
            List<bool> row = GetRow(y);
            CreateHintLine(GetHints(row), false, y, startPosition);
        }
        CheckSolved();
    }

    private void CreateHintLine(List<Hint> hints, bool isCol, int position, Vector2 startPosition) {
        GameObject hintSection = isCol ? GameObject.Find("HintCols") : GameObject.Find("HintRows");
        GameObject hintBG = isCol ? GameObject.Find("HintColsBG") : GameObject.Find("HintRowsBG");
        float boxSize = GameObject.Find("Grid").GetComponent<GridController>().boxSize;

        GameObject hintLine = Instantiate(hintLinePrefab, hintBG.transform.position, hintLinePrefab.transform.rotation);
        hintLine.transform.SetParent(hintSection.transform);

        Vector2 updatedPosition;
        Vector2 updatedScale;
        int lineSize;
        if (isCol) {
            //updatedPosition = new Vector2(hintBG.transform.position.x + (position * boxSize) - (hintBG.transform.localScale.x / 2) + (boxSize / 2),
            //                              hintBG.transform.position.y);
            updatedPosition = new Vector2(startPosition.x + (position * boxSize),
                                          hintBG.transform.position.y);
            updatedScale = new Vector2(boxSize, hintBG.transform.localScale.y);
            hintLine.transform.position = updatedPosition;
            hintLine.transform.localScale = updatedScale;
            lineSize = puzzle.Count;
        } else {
            //updatedPosition = new Vector2(hintBG.transform.position.x,
            //                              hintBG.transform.position.y - (position * boxSize) + (hintBG.transform.localScale.y / 2) - (boxSize / 2));
            updatedPosition = new Vector2(hintBG.transform.position.x,
                                          startPosition.y - (position * boxSize));
            updatedScale = new Vector2(hintBG.transform.localScale.x, boxSize);
            hintLine.transform.position = updatedPosition;
            hintLine.transform.localScale = updatedScale;
            lineSize = puzzle[0].Count;
        }
        HintLine line = hintLine.GetComponent<HintLine>();
        line.Init(hints, isCol, position, position == 0, lineSize);
        if (isCol)
        {
            hintColumns.Add(line);
        }
        else
        {
            hintRows.Add(line);
        }
    }

    List<Hint> GetHints(List<bool> puzzleLine) {
        List<Hint> hints = new();
        int num = 0;
        for (int i = 0; i < puzzleLine.Count; i++) {
            if (puzzleLine[i]) {
                num++;
            } else {
                if (num > 0) {
                    hints.Add(new Hint(num, false));
                    num = 0;
                }
            }
        }
        if (hints.Count == 0 || num > 0) {
            hints.Add(new Hint(num, false)); //bookends with last element, also covers 0 case
        }
        return hints;
    }

    private void OnBoxUpdated(int stateUpdate, int col, int row, List<List<int>> lineData)
    {
        hintColumns[col].solved = hintColumns[col].CalculateSolved(lineData[0]);
        hintRows[row].solved = hintRows[row].CalculateSolved(lineData[1]);
        
        if (CheckSolved())
        {
            Debug.Log("DEBUG PuzzleController.CheckSolved: Puzzle complete!");
        }
        hintColumns[col].CheckCaptures(lineData[0]);
        hintRows[row].CheckCaptures(lineData[1]);
    }

    private bool CheckSolved()
    {
        bool solved = true;
        for (int i = 0; i < hintColumns.Count; i++)
        {
            if (!hintColumns[i].solved)
            {
                solved = false;
            }
        }
        for (int i = 0; i < hintRows.Count; i++)
        {
            if (!hintRows[i].solved)
            {
                solved = false;
            }
        }
        return solved;
    }

    private void OnDestroy()
    {
        EventSystem.current.onBoxUpdated -= OnBoxUpdated;
    }
}
