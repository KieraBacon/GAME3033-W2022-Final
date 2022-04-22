using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum MenuIndices
    {
        MainSubmenu = 0,
        InstructionsSubmenu = 1,
        CreditsSubmenu = 2,
        PauseSubmenu = 3,
        VictorySubmenu = 4,
        LossSubmenu = 5,
        InGameUI = 6,
    }

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("Game Manager").AddComponent<GameManager>();

            return _instance;
        }
    }

    [SerializeField]
    private SubmenuSwitcher menuSwitcher;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Start()
    {
        TimeManager.shallowPaused = true;
        menuSwitcher.gameObject.SetActive(true);
        menuSwitcher.SwitchSubmenu(0);
    }

    public static void StartGame()
    {
        TimeManager.ResetTime();
        foreach (GlowbugSpawner glowbugSpawner in GlowbugSpawner.allGlowbugSpawners)
            glowbugSpawner.SpawnAll();
        foreach (PlayerController playerController in PlayerController.allPlayerControllers)
            playerController.StartGame();

        ResumeGame();
    }

    public static void EndGame()
    {
        foreach (PlayerController playerController in PlayerController.allPlayerControllers)
            playerController.EndGame();
    }

    public static void PauseGame()
    {
        TimeManager.shallowPaused = true;
        foreach (PlayerController playerController in PlayerController.allPlayerControllers)
            playerController.allowInput = false;
        GameManager gm = instance;
        gm.menuSwitcher.gameObject.SetActive(true);
        gm.menuSwitcher.SwitchSubmenu(4);
    }

    public static void ResumeGame()
    {
        TimeManager.shallowPaused = false;
        foreach (PlayerController playerController in PlayerController.allPlayerControllers)
            playerController.allowInput = true;
        GameManager gm = instance;
        gm.menuSwitcher.Close();
    }
}
