using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
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

    public GameObject gameMenu;

    private ControlData controls;

    void Start()
    {
        SetFields();
        SetUpGame();
    }

    private void SetFields()
    {
        if (gridScript == null)
        {
            gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        }
        if (puzzleScript == null)
        {
            puzzleScript = GameObject.Find("Puzzle").GetComponent<PuzzleController>();
            puzzleScript.RegisterListener();
        }
    }

    public void SetUpGame()
    {
        //TODO: these should be constants.
        controls = new ControlData(0.45f, 0.15f);

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
        }
        else
        {
            Debug.Log("Invalid start flow. Leaving this in for debugging purposes but this should not be normally accessed.");
            gridScript.gameDimensionX = gameDimensionX;
            gridScript.gameDimensionY = gameDimensionY;
            puzzleScript.CreatePuzzle(gridScript.gameDimensionX, gridScript.gameDimensionY, Constants.DIFFICULTY_MEDIUM, gridScript.startPosition);
        }

        ClearScenePersistence();

        gridScript.SetUpGrid();

        float boxSize = gridScript.boxSize;

        GameObject cursor = Instantiate(cursorPrefab, gridScript.startPosition, cursorPrefab.transform.rotation);
        cursor.transform.localScale = new Vector2(boxSize * 2, boxSize * 2);
        cursorScript = cursor.GetComponent<CursorController>();
    }

    private void ClearScenePersistence()
    {
        ScenePersistence.base64Code = null;
        ScenePersistence.difficulty = 0;
    }

    void Update()
    {
        controls.SetKeyState();
        
        //TODO: i think it's time to refactor controls into an event system, not update every frame.
        if (controls.isPaused)
        {
            gameMenu.SetActive(true);
            return;
        } else
        {
            CloseMenu();
        }

        controls.Move(CommitMove);
        (int x, int y) = cursorScript.GetGamePosition();
        controls.Action(x, y, gridScript.SetCellState, gridScript.OverwriteCellState);
    }

    public void CloseMenu()
    {
        gameMenu.SetActive(false);
        controls.isPaused = false;
    }

    public void ActivateMenu()
    {
        gameMenu.SetActive(true);
        //TODO: need a better flow for this. resetting this value to have gamemenucontroller not interfere on victory screen.
        //... or the victory screen needs to be different from the pause menu. bold concept
        controls.isPaused = true;
    }

    private void CommitMove(int move)
    {
        float boxSize = gridScript.boxSize;

        //TODO: refactor Move function's parameters
        switch (move)
        {
            case Constants.KEY_LEFT:
                cursorScript.Move(gridScript.gameDimensionX, gridScript.gameDimensionY, boxSize, xMove: -1);
                break;
            case Constants.KEY_RIGHT:
                cursorScript.Move(gridScript.gameDimensionX, gridScript.gameDimensionY, boxSize, xMove: 1);
                break;
            case Constants.KEY_UP:
                cursorScript.Move(gridScript.gameDimensionX, gridScript.gameDimensionY, boxSize, yMove: -1);
                break;
            case Constants.KEY_DOWN:
                cursorScript.Move(gridScript.gameDimensionX, gridScript.gameDimensionY, boxSize, yMove: 1);
                break;
            default:
                break;
        }
    }
}
