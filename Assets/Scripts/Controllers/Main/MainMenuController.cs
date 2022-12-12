using Assets.Scripts.Constants;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public void NewPuzzle()
    {
        string input = TrimInput(GameObject.Find("WidthInputText").GetComponent<TextMeshProUGUI>().text);
        ScenePersistence.width = Convert.ToInt32(input);
        input = TrimInput(GameObject.Find("HeightInputText").GetComponent<TextMeshProUGUI>().text);
        ScenePersistence.height = Convert.ToInt32(input);
        string name = GameObject.Find("DifficultyPanel").GetComponent<ToggleGroup>().GetFirstActiveToggle().gameObject.name;
        switch (name) {
            case "EasyRadioButton":
                ScenePersistence.difficulty = Constants.DIFFICULTY_EASY;
                break;
            case "MediumRadioButton":
                ScenePersistence.difficulty = Constants.DIFFICULTY_MEDIUM;
                break;
            case "HardRadioButton":
                ScenePersistence.difficulty = Constants.DIFFICULTY_HARD;
                break;
            default:
                Debug.Log("MainMenuController.NewPuzzle: WARN No difficulty selected");
                break;
        }
        if (ValidateNewPuzzle())
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void RandomPuzzle()
    {
        ScenePersistence.width = UnityEngine.Random.Range(5, 21);
        ScenePersistence.height = UnityEngine.Random.Range(5, 21);
        int[] difficulties = new int[3] { Constants.DIFFICULTY_EASY, Constants.DIFFICULTY_MEDIUM, Constants.DIFFICULTY_HARD };
        ScenePersistence.difficulty = difficulties[UnityEngine.Random.Range(0, 3)];
        SceneManager.LoadScene("GameScene");
    }

    public void LoadPuzzle()
    {
        string code = GameObject.Find("LoadPuzzleCodeText").GetComponent<TextMeshProUGUI>().text;
        code = TrimInput(code);
        if (ValidateLoadPuzzle(code))
        {
            Debug.Log("MainMenuController.LoadPuzzle: Valid puzzle, loading");
            ScenePersistence.base64Code = code;
            SceneManager.LoadScene("GameScene");
            //manage scene to transition to game, which launches with code if not empty
        }
    }

    public void DrawPuzzle()
    {
        string input = TrimInput(GameObject.Find("WidthInputText").GetComponent<TextMeshProUGUI>().text);
        ScenePersistence.width = Convert.ToInt32(input);
        input = TrimInput(GameObject.Find("HeightInputText").GetComponent<TextMeshProUGUI>().text);
        ScenePersistence.height = Convert.ToInt32(input);
        if (ValidateDrawPuzzle())
        {
            SceneManager.LoadScene("DrawScene");
        }
    }

    //input fields seem to have an invisible trailing character that's causing problems.
    private string TrimInput(string input)
    {
        return input.Substring(0, input.Length - 1);
    }

    private bool ValidateNewPuzzle()
    {
        string errors = "";
        if (ScenePersistence.width == 0)
        {
            errors += "Width missing.\n";
        }
        if (ScenePersistence.height == 0)
        {
            errors += "Height missing.\n";
        }
        if (ScenePersistence.difficulty == 0)
        {
            errors += "Difficulty missing.\n";
        }
        if (errors.Length > 0)
        {
            Debug.Log($"MainMenuController.ValidateNewPuzzle: errors: {errors}");
            return false;
        }
        return true;
    }
    private bool ValidateLoadPuzzle(string code)
    {
        string errors = "";
        if (code == null || code.Length == 0 || code.Length == 1)
        {
            errors += "Code missing\n";
        }
        if (!int.TryParse(code.Substring(0, 2), out int width))
        {
            errors += "Invalid width\n";
        }
        if (!int.TryParse(code.Substring(2, 2), out int height))
        {
            errors += "Invalid height\n";
        }
        string base64Code = code[4..];
        Span<byte> buffer = new Span<byte>(new byte[base64Code.Length]);
        Debug.Log(Convert.TryFromBase64String(code[4..], buffer, out int bytesParsedA));
        if (!Convert.TryFromBase64String(code[4..], buffer, out int bytesParsed))
        {
            errors += "Invalid puzzle code\n";
        }

        if (errors.Length > 0)
        {
            Debug.Log($"MainMenuController.ValidateLoadPuzzle Errors: {errors}");
            Debug.Log("MainMenuController.ValidateLoadPuzzle Error: Not loading scene.");
            return false;
        } else
        {
            return true;
        }
    }
    private bool ValidateDrawPuzzle()
    {
        string errors = "";
        if (ScenePersistence.width == 0)
        {
            errors += "Width missing.\n";
        }
        if (ScenePersistence.height == 0)
        {
            errors += "Height missing.\n";
        }
        if (errors.Length > 0)
        {
            Debug.Log($"MainMenuController.ValidateNewPuzzle: errors: {errors}");
            return false;
        }
        return true;
    }
}
