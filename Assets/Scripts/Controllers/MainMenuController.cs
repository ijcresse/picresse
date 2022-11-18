using Assets.Scripts.Constants;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class MainMenuController : MonoBehaviour
{
    public void LoadPuzzle()
    {
        string code = GameObject.Find("LoadPuzzleCodeText").GetComponent<TextMeshProUGUI>().text;
        code = code.Substring(0, code.Length - 1); //trim trailing weird char
        if (ValidateLoadPuzzle(code))
        {
            Debug.Log("MainMenuController.LoadPuzzle: Valid puzzle, loading");
            ScenePersistence.base64Code = code;
            SceneManager.LoadScene("GameScene");
            //manage scene to transition to game, which launches with code if not empty
        }
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
}
