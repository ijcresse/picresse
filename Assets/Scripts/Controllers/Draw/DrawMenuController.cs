using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ExitDraw()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void CopyToClipboard()
    {
        string puzzleCode = GameObject.Find("Puzzle").GetComponent<PuzzleController>().puzzleCode;
        GUIUtility.systemCopyBuffer = puzzleCode;
    }

    /*
     * pause detection - show menu
     * 
     * edit mode button
     * back to main menu button
     *  * show warning to user about quitting without code
     *  * offer user "quit" "return" "copy to clipboard"
     */
    
}
