using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool[,] puzzle;
    private GameObject[] hintColumns;
    private GameObject[] hintRows;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreatePuzzle(int x, int y) {
        Debug.Log($"PuzzleController.CreatePuzzle called for puzzle with size {x},{y}");
        puzzle = new bool[y, x];
        for (int i = 0; i < y; i++) {
            for (int j = 0; j < x; j++) {
                puzzle[i, j] = Random.Range(0f, 1f) <= 0.6f;
            }
            string line = "";
            for (int j = 0; j < x; j++) {
                line = line + (puzzle[i,j] ? 'o' : 'x');
            }
            Debug.Log(line);
        }
    }

    public void CreatePuzzle(string base64Code) {

    }


}
