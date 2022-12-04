using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject victoryPanel;

    public void Start()
    {
        EventSystem.current.onAlertWin += OnAlertWin;
    }

    public void OnAlertWin()
    {
        GameObject.Find("GameBoard").GetComponent<GameController>().isGameActive = false;
        victoryPanel.SetActive(true);
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
        SceneManager.LoadScene("DrawScene");
    }
    
    public void SharePuzzle()
    {
        string puzzleCode = GameObject.Find("Puzzle").GetComponent<PuzzleController>().puzzleCode;
        GUIUtility.systemCopyBuffer = puzzleCode;
        //TODO: add alert saying copied to clipboard. which means add alert system i guess
    }

    public void OnDestroy()
    {
        EventSystem.current.onAlertWin -= OnAlertWin;
    }
}
