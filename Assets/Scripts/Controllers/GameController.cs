using Assets.Scripts.Constants;
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
        puzzleScript = GameObject.Find("Puzzle").GetComponent<PuzzleController>();

        if (ScenePersistence.base64Code != null)
        {
            gridScript.gameDimensionX = System.Convert.ToInt32(ScenePersistence.base64Code.Substring(0, 2));
            gridScript.gameDimensionY = System.Convert.ToInt32(ScenePersistence.base64Code.Substring(2, 2));
            puzzleScript.CreatePuzzle(ScenePersistence.base64Code, gridScript.startPosition);
        }
        else if (ScenePersistence.difficulty != 0)
        {
            gridScript.gameDimensionX = ScenePersistence.width;
            gridScript.gameDimensionY = ScenePersistence.height;
            puzzleScript.CreatePuzzle(gridScript.gameDimensionX, gridScript.gameDimensionY, ScenePersistence.difficulty, gridScript.startPosition);
        } else
        {
            Debug.Log("Invalid start flow. Leaving this in for debugging purposes but this should not be normally accessed.");
            gridScript.gameDimensionX = gameDimensionX;
            gridScript.gameDimensionY = gameDimensionY;
            puzzleScript.CreatePuzzle(gridScript.gameDimensionX, gridScript.gameDimensionY, Constants.DIFFICULTY_MEDIUM, gridScript.startPosition);
        }

        gridScript.SetUpGrid();

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
