using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GamePaused { get { return gamePaused; } }

    private static bool gamePaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private StartSimulationPanelScript startSimScript;

    [SerializeField]
    private KeyCode pauseActivationKey;

    void Update()
    {
        if(Input.GetKeyUp(pauseActivationKey))
        {
            if(GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void RestartSimulation()
    {
        //usunięcie wszystkiego na scenie

        Resume();

        startSimScript.ShowPanel();
        startSimScript.StopSimulation();
        startSimScript.DespawnSimulationObjects();
    }
}
