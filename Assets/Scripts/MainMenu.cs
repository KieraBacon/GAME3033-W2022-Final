using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        TimeManager.ResetTime();
        foreach (GlowbugSpawner glowbugSpawner in GlowbugSpawner.allGlowbugSpawners)
            glowbugSpawner.SpawnAll();
        foreach (PlayerController playerController in PlayerController.allPlayerControllers)
            playerController.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
