using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
public class GameMenuController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendTextToBrowser(string text);

    public GameObject victoryPanel;

    public void Start()
    {
        EventSystem.current.onAlertWin += OnAlertWin;
    }

    public void OnAlertWin()
    {
        GameObject.Find("GameBoard").GetComponent<GameController>().ActivateMenu();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    //TODO: this is copy pasted from mainmenucontroller :\
    public void NewRandomGame()
    {
        ScenePersistence.width = Random.Range(5, 21);
        ScenePersistence.height = Random.Range(5, 21);
        int[] difficulties = new int[3] { Constants.DIFFICULTY_EASY, Constants.DIFFICULTY_MEDIUM, Constants.DIFFICULTY_HARD };
        ScenePersistence.difficulty = difficulties[Random.Range(0, 3)];
        SceneManager.LoadScene("GameScene");
    }
    
    public void SharePuzzle()
    {
        string puzzleCode = GameObject.Find("Puzzle").GetComponent<PuzzleController>().puzzleCode;
        SendTextToBrowser(puzzleCode);
        //GUIUtility.systemCopyBuffer = puzzleCode;
    }

    public void OnDestroy()
    {
        EventSystem.current.onAlertWin -= OnAlertWin;
    }
}
