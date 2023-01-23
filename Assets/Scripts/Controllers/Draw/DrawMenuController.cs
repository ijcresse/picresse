using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class DrawMenuController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendTextToBrowser(string text);

    public void ExitDraw()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void CopyToClipboard()
    {
        string puzzleCode = GameObject.Find("Puzzle").GetComponent<PuzzleController>().puzzleCode;
        SendTextToBrowser(puzzleCode);
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
