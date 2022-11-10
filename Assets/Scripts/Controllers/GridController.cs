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

    void DebugLog(string s)
    {
        Debug.Log("GridController: " + s);
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
                pixOffset *= gridSpriteSize.x;
                startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2 + pixOffset,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);

                /*
                GameObject hintColsUI = GameObject.Find("HintColsUI");
                HorizontalLayoutGroup verticalLayout = hintColsUI.GetComponent<HorizontalLayoutGroup>();
                RectOffset padding = verticalLayout.padding;
                padding = new RectOffset((int) pixOffset, (int) pixOffset, padding.top, padding.bottom);
                verticalLayout.padding = padding;
                */

                /*
                GameObject hintColsBG = GameObject.Find("HintColsBG");
                Transform cam = Camera.main.transform;
                Vector2 worldPt = hintColsBG.transform.position;
                worldPt = new Vector2(worldPt.x + pixOffset, worldPt.y); //bottom left edge
                worldPt = cam.TransformPoint(worldPt);
                hintColsUI.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPt);
                Debug.Log(worldPt);
                */
                //Component verticalLayout = hintColsUI.GetComponent<UI>();
            }
            else
            {
                boxSize = gridSpriteSize.x / gameDimensionX;
                pixOffset = 1f - (float) gameDimensionY / (float) gameDimensionX;
                pixOffset *= 0.5f;
                pixOffset *= gridSpriteSize.y;
                startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2,
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2 - pixOffset);
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
                
                Vector2 currentPosition = new Vector2(startPosition.x + (col * boxSize), startPosition.y - (row * boxSize));
                GameObject box = Instantiate(boxPrefab, currentPosition, boxPrefab.transform.rotation);
                box.transform.SetParent(gameObject.transform, false);

                //ok, i think this is where things are going wrong with the box offset
                box.transform.localScale = new Vector2(boxSize * 1.9f, boxSize * 1.9f);
                grid[col].Add(box);
            }
        }
    }
}
