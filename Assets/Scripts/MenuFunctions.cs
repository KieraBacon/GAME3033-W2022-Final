using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.StartGame();
    }

    public void PauseGame()
    {
        GameManager.PauseGame();
    }

    public static void ResumeGame()
    {
        GameManager.ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
