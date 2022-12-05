using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public List<List<bool>> puzzle;
    private List<HintLine> hintColumns;
    private List<HintLine> hintRows;
    public GameObject hintLinePrefab;
    public string puzzleCode { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RegisterListener()
    {
        EventSystem.current.onBoxUpdated += OnBoxUpdated;
    }

    public void CreatePuzzle(int x, int y, int difficulty, Vector2 startPosition) {
        puzzle = new List<List<bool>>();
        for (int i = 0; i < y; i++) {
            string line = "";
            puzzle.Add(new List<bool>());
            for (int j = 0; j < x; j++) {
                puzzle[i].Add(Random.Range(0f, 1f) <= difficulty * 0.15f);
                line += puzzle[i][j] ? 'o' : 'x';
            }
        }
        CreateHints(startPosition);
        GeneratePuzzleString();
        GameObject.Find("ClockUI").GetComponent<TextMeshProUGUI>().text = puzzleCode;
    }

    public void CreatePuzzle(string base64Code, Vector2 startPosition) {
        List<List<bool>> generatedPuzzle = new();

        int cols = System.Convert.ToInt32(base64Code.Substring(0, 2));
        int rows = System.Convert.ToInt32(base64Code.Substring(2, 2));
        byte[] arr = System.Convert.FromBase64String(base64Code[4..]);
        BitArray bits = new(arr);

        int count = 0;
        for (int i = 0; i < cols; i++)
        {
            generatedPuzzle.Add(new List<bool>());
            for (int j = 0; j < rows; j++)
            {
                generatedPuzzle[i].Add(bits[count++]);
            }
        }
        puzzle = generatedPuzzle;
        CreateHints(startPosition);
        GeneratePuzzleString();
        GameObject.Find("ClockUI").GetComponent<TextMeshProUGUI>().text = puzzleCode;
    }

    private void GeneratePuzzleString()
    {
        puzzleCode = "";
        int cols = puzzle.Count;
        int rows = puzzle[0].Count;
        bool[] bits = puzzle.SelectMany(x => x).ToArray();
        byte[] bytes = new byte[(bits.Length + 7) / 8];
        byte bit = 0;
        int bitIndex = 0;
        int byteIndex = 0;
        foreach (bool box in bits)
        {
            byte b = (byte)(box ? 1 : 0);
            b <<= bitIndex++;
            bit |= b;
            if (bitIndex == 8)
            {
                bytes[byteIndex++] = bit;
                bitIndex = 0;
                bit = 0;
            }
        }
        if (bitIndex > 0)
        {
            bytes[byteIndex] = bit;
        }
        puzzleCode = cols.ToString("00") + rows.ToString("00") + System.Convert.ToBase64String(bytes);
    }

    public bool GeneratePuzzleString(List<List<bool>> grid)
    {
        puzzle = grid;
        GeneratePuzzleString();
        return true;
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
            updatedPosition = new Vector2(startPosition.x + (position * boxSize),
                                          hintBG.transform.position.y);
            updatedScale = new Vector2(boxSize, hintBG.transform.localScale.y);
            hintLine.transform.position = updatedPosition;
            hintLine.transform.localScale = updatedScale;
            lineSize = puzzle.Count;
        } else {
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
            EventSystem.current.AlertWin();   
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
