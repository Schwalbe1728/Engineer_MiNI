using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
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
    private PopulationManagerScript populationManager;

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
        Resume();

        startSimScript.ShowPanel();
        startSimScript.StopSimulation();
        startSimScript.DespawnSimulationObjects();
    }

    public void SaveLearningProcess()
    {
        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            switch (sfd.ShowDialog())
            {
                case DialogResult.OK:
                    Debug.Log(sfd.FileName);

                    populationManager.SaveLearningProcess(sfd.FileName);

                    break;
            }
        }
    }    
}
