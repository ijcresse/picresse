using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int gameDimensionX {get; set;}
    public int gameDimensionY {get; set;}
    public GameObject boxPrefab;
    private GameObject [,] grid;
    private GameObject gridSprite;
    private (float x, float y) gridSpriteSize;
    public float boxSize { get; set; }
    public Vector2 startPosition { get; set; }
    
    void Start()
    {
        gridSprite = GameObject.Find("GridBG");
        gridSpriteSize = (x: gridSprite.transform.localScale.x, y: gridSprite.transform.localScale.y);
    }

    void Update() {

    }

    public void SetCellState(int x, int y, bool isSpace) {
        if (x < 0 || y < 0 || x > gameDimensionX - 1 || y > gameDimensionY - 1) {
            DebugLog($"SetCell: ERROR: gameDimensions are not viable coordinates: {x}, {y}");
        }
        //get specific box's controller. y,x due to column-first structure
        BoxController box = grid[y, x].GetComponent<BoxController>();
        if (box.GetState() == 0) {
            if (isSpace) {
                box.SetState(1);
            } else {
                box.SetState(2);
            }
        } else if (box.GetState() == 1 || box.GetState() == 2) {
            box.SetState(0);
        }
    }

    // int GetCellState(int x, int y) {
    //     if (x < 0 || y < 0 || x > gameDimensionX - 1 || y > gameDimensionY - 1) {
    //         DebugLog($"SetCell: ERROR: gameDimensions are not viable coordinates: {x}, {y}");
    //         return -1;
    //     }
    //     return grid[x,y].GetState();
    // }

    void DebugLog(string s) {
        Debug.Log("GridController: " + s);
    }

    public void SetUpGrid() {
        boxSize = gridSpriteSize.y / gameDimensionY; //always compare one direction to get a square. also, y is generally smaller (25 x 20)
                                                     //later... fix this so it compares the smallest size, so we can better support variable sizes
        DebugLog($"boxSize: {boxSize}, gridSpriteSize: ({gridSpriteSize.x}, {gridSpriteSize.y})");
        startPosition = new Vector2(gridSprite.transform.position.x - (gridSpriteSize.x / 2) + boxSize / 2, 
                                            gridSprite.transform.position.y + (gridSpriteSize.y / 2) - boxSize / 2);

        grid = new GameObject[gameDimensionX, gameDimensionY];
        for (int i = 0; i < gameDimensionY; i++) {
            for (int j = 0; j < gameDimensionX; j++) {
                Vector2 currentPosition = new Vector2(startPosition.x + (i * boxSize), startPosition.y - (j * boxSize));
                GameObject box = Instantiate(boxPrefab, currentPosition, boxPrefab.transform.rotation);
                box.transform.SetParent(gameObject.transform, false);
                //TODO: better name pls
                double temp = boxSize * 2;
                box.transform.localScale = new Vector2((float) temp, (float) temp);
                grid[j, i] = box;
            }
        }
    }
}
