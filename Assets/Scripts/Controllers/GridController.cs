using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //sets the new cell state.
    //new state depends on the current state of the box.
    public int SetCellState(int col, int row, int action)
    {
        if (col < 0 || row < 0 || col > gameDimensionX - 1 || row > gameDimensionY - 1)
        {
            Debug.Log($"GridController.SetCell ERROR: gameDimensions are not viable coordinates: {col}, {row}");
        }
        BoxController box = grid[col][row].GetComponent<BoxController>();
        int stateUpdate = -1;
        if (box.GetState() == Constants.INACTIVE)
        {
            /*
            if (action == Constants.KEY_FILL)
            {
                stateUpdate = Constants.ACTIVE;
            }
            else if (action == Constants.KEY_CROSS)
            {
                stateUpdate = Constants.CROSSED;
            }
            */
            stateUpdate = action;
        }
        else if (box.GetState() == Constants.ACTIVE || box.GetState() == Constants.CROSSED)
        {
            stateUpdate = Constants.INACTIVE;
        }
        box.SetState(stateUpdate);
        EventSystem.current.BoxUpdated(stateUpdate, col, row, GetGridLines(col, row));
        return stateUpdate;
    }

    //overwrites to the new state.
    //for moving while an action key is held down.
    public void OverwriteCellState(int col, int row, int action)
    {
        if (col < 0 || row < 0 || col > gameDimensionX - 1 || row > gameDimensionY - 1)
        {
            Debug.Log($"GridController.SetCell ERROR: gameDimensions are not viable coordinates: {col}, {row}");
        }
        BoxController box = grid[col][row].GetComponent<BoxController>();
        int stateUpdate = -1;
        switch (action)
        {
            case Constants.KEY_FILL:
                stateUpdate = Constants.ACTIVE;
                break;
            case Constants.KEY_CROSS:
                stateUpdate = Constants.CROSSED;
                break;
            case Constants.KEY_CLEAR:
                stateUpdate = Constants.INACTIVE;
                break;
            default:
                Debug.Log($"GridController.OverwriteCell ERROR: stateUpdate invalid value: {stateUpdate}");
                break;
        }
        box.SetState(stateUpdate);
        EventSystem.current.BoxUpdated(stateUpdate, col, row, GetGridLines(col, row));
    }

    public List<List<int>> GetGridLines(int col, int row)
    {
        List<int> colStates = new();
        for (int i = 0; i < gameDimensionY; i++)
        {
            colStates.Add(grid[col][i].GetComponent<BoxController>().GetState());
        }
        List<int> rowStates = new();
        for (int i = 0; i < gameDimensionX; i++)
        {
            rowStates.Add(grid[i][row].GetComponent<BoxController>().GetState());
        }
        return new List<List<int>> { colStates, rowStates };
    }
    public void SetUpGrid()
    {
        bool sidesSame = gameDimensionX == gameDimensionY;
        float pixOffset;
        if (!sidesSame)
        {
            if (gameDimensionX < gameDimensionY)
            {
                boxSize = gridSpriteSize.y / gameDimensionY;
                pixOffset = 1f - (float) gameDimensionX / (float) gameDimensionY;
                pixOffset *= 0.5f;
                startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2 + pixOffset * gridSpriteSize.x,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);

                HorizontalLayoutGroup layout = GameObject.Find("HintColsUI").GetComponent<HorizontalLayoutGroup>();
                RectOffset padding = layout.padding;
                pixOffset *= layout.GetComponent<RectTransform>().rect.width;
                padding = new RectOffset((int)pixOffset, (int)pixOffset, 0, 0);
                layout.padding = padding;
            }
            else
            {
                boxSize = gridSpriteSize.x / gameDimensionX;
                pixOffset = 1f - (float) gameDimensionY / (float) gameDimensionX;
                pixOffset *= 0.5f;
                startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2 - pixOffset * gridSpriteSize.y);

                
                VerticalLayoutGroup layout = GameObject.Find("HintRowsUI").GetComponent<VerticalLayoutGroup>();
                RectOffset padding = layout.padding;
                pixOffset *= layout.GetComponent<RectTransform>().rect.height;
                padding = new RectOffset(0, 0, (int)pixOffset, (int)pixOffset);
                layout.padding = padding;
                

            }
        } else
        {
            boxSize = gridSpriteSize.y / gameDimensionY;
            startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                        gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);
        }

        grid = new List<List<GameObject>>();
        for (int col = 0; col < gameDimensionX; col++)
        {
            grid.Add(new List<GameObject>());
            for (int row = 0; row < gameDimensionY; row++)
            {
                GameObject box = Instantiate(boxPrefab, new Vector2(), boxPrefab.transform.rotation);
                BoxController controller = box.GetComponent<BoxController>();
                controller.x = col;
                controller.y = row;
                box.transform.SetParent(gameObject.transform, false);

                box.transform.localScale = new Vector2(boxSize * 1.9f, boxSize * 1.9f);
                Vector2 currentPosition = new Vector2(startPosition.x + (col * boxSize), startPosition.y - (row * boxSize));
                box.transform.position = currentPosition;
                grid[col].Add(box);
            }
        }
    }
}
