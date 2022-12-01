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
    public bool isGameActive;

    private ControlData controls;

    //reference to last updated box, to prevent repeated lookups
    //TODO: look for more elegant solution
    private (int x, int y) lastUpdatedPositions;
    private int lastAction;
    private int lastActionResult;

    void Start()
    {
        //TODO: these should be constants. 
        controls = new ControlData(0.5f, 0.25f);

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

        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive)
        {
            return;
        }

        SetKeyState();

        int move = controls.GetMove();
        if (move != -1)
        {
            if (controls.repeatTimer == 0f)
            {
                controls.firstMovePress = true;
            }
            controls.repeatTimer += Time.deltaTime;
        } else
        {
            controls.repeatTimer = 0f;
        }

        if (controls.firstMovePress)
        {
            CommitMove(move);
        } else if (controls.repeatTimer > controls.timerThreshold)
        {
            if (controls.repeatTimer > controls.repeatSpeed)
            {
                //prevent going too high, for some god forbidden reason
                controls.repeatTimer = controls.timerThreshold;
                CommitMove(move);
            }
        }
        controls.firstMovePress = false;

        int action = controls.GetAction();
        if (action != -1)
        {
            CommitAction(action, move);
        }
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

    private void CommitAction(int action, int move)
    {
        (int x, int y) = cursorScript.GetGamePosition();
        //skip update if it's the same action on the same square.
        //no. we only want to skip on the same PRESS if it's the same action.
        if (controls.firstActionPress)
        {
            lastActionResult = gridScript.SetCellState(x, y, action);
            controls.firstActionPress = false;
        } else if ((lastUpdatedPositions.x != x || lastUpdatedPositions.y != y) || action != lastAction)
        {
            if (move != -1)
            {
                gridScript.OverwriteCellState(x, y, lastActionResult);
            } else
            {
                lastActionResult = gridScript.SetCellState(x, y, action);
            }
        }
        lastUpdatedPositions = (x, y);
        lastAction = action;
    }

    private void SetKeyState()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            controls.movementKey[Constants.KEY_LEFT] = true;
        } else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            controls.movementKey[Constants.KEY_LEFT] = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            controls.movementKey[Constants.KEY_RIGHT] = true;
        } else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            controls.movementKey[Constants.KEY_RIGHT] = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            controls.movementKey[Constants.KEY_UP] = true;
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            controls.movementKey[Constants.KEY_UP] = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            controls.movementKey[Constants.KEY_DOWN] = true;
        } else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            controls.movementKey[Constants.KEY_DOWN] = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controls.actionKey[Constants.KEY_FILL] = true;
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            controls.actionKey[Constants.KEY_FILL] = false;
            controls.firstActionPress = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controls.actionKey[Constants.KEY_CROSS] = true;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            controls.actionKey[Constants.KEY_CROSS] = false;
            controls.firstActionPress = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controls.actionKey[Constants.KEY_CLEAR] = true;
        } else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            controls.actionKey[Constants.KEY_CLEAR] = false;
            controls.firstActionPress = true;
        }
    }
}
