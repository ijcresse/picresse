using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using UnityEngine;
using TMPro;

public class DrawController : MonoBehaviour
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
    public bool isDrawingActive;

    private ControlData controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new ControlData(0.3f, 0.08f);

        gridScript = GameObject.Find("Grid").GetComponent<GridController>();
        puzzleScript = GameObject.Find("Puzzle").GetComponent<PuzzleController>();

        if (ScenePersistence.width != 0 || ScenePersistence.height != 0)
        {
            gridScript.gameDimensionX = ScenePersistence.width;
            gridScript.gameDimensionY = ScenePersistence.height;
        } else
        {
            Debug.Log("Invalid start flow. Leaving this in for debugging purposes but this should not be normally accessed.");
            gridScript.gameDimensionX = gameDimensionX;
            gridScript.gameDimensionY = gameDimensionY;
        }

        gridScript.SetUpGrid();

        float boxSize = gridScript.boxSize;

        GameObject cursor = Instantiate(cursorPrefab, gridScript.startPosition, cursorPrefab.transform.rotation);
        cursor.transform.localScale = new Vector2(boxSize * 2, boxSize * 2);
        cursorScript = cursor.GetComponent<CursorController>();

        isDrawingActive = true;
    }

    void Update()
    {
        controls.SetKeyState();
        //only allow fill/clear
        controls.actionKey[Constants.KEY_CROSS] = false;

        controls.Move(CommitMove);
        (int x, int y) = cursorScript.GetGamePosition();
        
        controls.Action(x, y, gridScript.SetCellState, gridScript.OverwriteCellState);
    }

    private void CommitMove(int move)
    {
        float boxSize = gridScript.boxSize;

        switch (move)
        {
            case Constants.KEY_LEFT:
                cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: -1);
                break;
            case Constants.KEY_RIGHT:
                cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, xMove: 1);
                break;
            case Constants.KEY_UP:
                cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: -1);
                break;
            case Constants.KEY_DOWN:
                cursorScript.Move(gameDimensionX, gameDimensionY, boxSize, yMove: 1);
                break;
            default:
                break;
        }
    }

    //triggered via the button in the clock area 
    public void CreatePuzzleCode()
    {
        bool complete = false;
        while (!complete)
        {
            complete = puzzleScript.GeneratePuzzleString(gridScript.GetGridState());
        }
        GameObject.Find("DrawCodeText").GetComponent<TextMeshProUGUI>().text = puzzleScript.puzzleCode;
        GUIUtility.systemCopyBuffer = puzzleScript.puzzleCode;
    }
}
