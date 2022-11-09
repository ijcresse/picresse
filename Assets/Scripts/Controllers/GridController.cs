using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int gameDimensionX { get; set; }
    public int gameDimensionY { get; set; }
    public GameObject boxPrefab;
    private List<List<GameObject>> grid;
    private GameObject gridSprite;
    private (float x, float y) gridSpriteSize;
    public float boxSize { get; set; }
    public Vector2 startPosition { get; set; }

    void Start()
    {
        gridSprite = GameObject.Find("GridBG");
        gridSpriteSize = (x: gridSprite.transform.localScale.x, y: gridSprite.transform.localScale.y);
    }

    void Update()
    {

    }

    public void SetCellState(int col, int row, bool isSpace)
    {
        if (col < 0 || row < 0 || col > gameDimensionX - 1 || row > gameDimensionY - 1)
        {
            DebugLog($"SetCell: ERROR: gameDimensions are not viable coordinates: {col}, {row}");
        }
        BoxController box = grid[col][row].GetComponent<BoxController>();
        int stateUpdate = -1;
        if (box.GetState() == Constants.INACTIVE)
        {
            if (isSpace)
            {
                stateUpdate = Constants.ACTIVE;
            }
            else
            {
                stateUpdate = Constants.CROSSED;
            }
        }
        else if (box.GetState() == Constants.ACTIVE || box.GetState() == Constants.CROSSED)
        {
            stateUpdate = Constants.INACTIVE;
        }
        box.SetState(stateUpdate);
        EventSystem.current.BoxUpdated(stateUpdate, col, row, GetGridLines(col, row));
    }

    public List<List<int>> GetGridLines(int col, int row)
    {
        List<int> colStates = new();
        for (int i = 0; i < gameDimensionX; i++)
        {
            colStates.Add(grid[col][i].GetComponent<BoxController>().GetState());
        }
        List<int> rowStates = new();
        for (int i = 0; i < gameDimensionY; i++)
        {
            rowStates.Add(grid[i][row].GetComponent<BoxController>().GetState());
        }
        return new List<List<int>> { colStates, rowStates };
    }

    void DebugLog(string s)
    {
        Debug.Log("GridController: " + s);
    }

    public void SetUpGrid()
    {
        bool sidesSame = gameDimensionX == gameDimensionY;
        if (!sidesSame)
        {
            Debug.Log("GridController.SetUpGrid: sides not the same.");
            float pixOffset = 0;
            if (gameDimensionX < gameDimensionY)
            {
                boxSize = gridSpriteSize.y / gameDimensionY;
                Debug.Log($"boxsize for x<y: {boxSize}");
                pixOffset = (gridSpriteSize.x - boxSize * gameDimensionX) / 2;
                startPosition = new Vector2(gridSprite.transform.position.x - pixOffset,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);
            }
            else
            {
                boxSize = gridSpriteSize.x / gameDimensionX;
                Debug.Log($"boxsize for y<x: {boxSize}");
                pixOffset = (gridSpriteSize.y - boxSize * gameDimensionY) / 2;
                startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                            gridSprite.transform.position.y + pixOffset);
            }
        } else
        {
            boxSize = gridSpriteSize.y / gameDimensionY;
            startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                        gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);
        }

        /*
        boxSize = gridSpriteSize.y / gameDimensionY; //always compare one direction to get a square. also, y is generally smaller (25 x 20)
                                                     //later... fix this so it compares the smallest size, so we can better support variable sizes
        DebugLog($"boxSize: {boxSize}, gridSpriteSize: ({gridSpriteSize.x}, {gridSpriteSize.y})");
        startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);
        */


        grid = new List<List<GameObject>>();
        for (int col = 0; col < gameDimensionX; col++)
        {
            grid.Add(new List<GameObject>());
            for (int row = 0; row < gameDimensionY; row++)
            {
                
                Vector2 currentPosition = new Vector2(startPosition.x + (col * boxSize), startPosition.y - (row * boxSize));
                Debug.Log($"startPosition:({startPosition.x},{startPosition.y}). currentPosition: ({startPosition.x + (col * boxSize)},{startPosition.y - (row * boxSize)})");
                Debug.Log($"boxSize: {boxSize}. boxPrefab details: ({boxPrefab.transform.position.x},{boxPrefab.transform.position.y})");
                GameObject box = Instantiate(boxPrefab, currentPosition, boxPrefab.transform.rotation);
                box.transform.SetParent(gameObject.transform, false);

                //ok, i think this is where things are going wrong with the box offset
                box.transform.localScale = new Vector2(boxSize * 1.9f, boxSize * 1.9f);
                //box.transform.localScale = new Vector2(boxSize, boxSize);
                grid[col].Add(box);
            }
        }
    }
}
