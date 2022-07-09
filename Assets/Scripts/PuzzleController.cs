using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    private bool[,] puzzle;
    private GameObject[] hintColumns;
    private GameObject[] hintRows;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreatePuzzle(int x, int y) {
        Debug.Log($"PuzzleController.CreatePuzzle called for puzzle with size {x},{y}");
        
    }

    public void CreatePuzzle(string base64Code) {

    }


}
