using NeuralNetwork.Core.Learning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationListScript : MonoBehaviour
{
    private List<ProcessData> GenerationHistory;

    [SerializeField]
    private PopulationManagerScript PopulationManager;

    [SerializeField]
    private StartSimulationPanelScript StartSimulationMenu;

    [SerializeField]
    private GameObject GenerationPanelPrefab;

    public void SetGenerationHistory(List<ProcessData> list)
    {
        Debug.Log("SetGenerationHistory");

        GenerationHistory = list;

        Debug.Log("SetGenerationHistory - Sort");

        
        GenerationHistory.Sort(
            (p, q) => 
                (p.BestScore > q.BestScore)? -1 :
                ((p.BestScore < q.BestScore)? 1 : 0)
            );
        
        Debug.Log("SetGenerationHistory - Reset");
        ResetList();
        
        int i = 0;
        Debug.Log("SetGenerationHistory - Create New Children");
        foreach (ProcessData data in GenerationHistory)
        {
            CreateGenerationPanel(data, i++);
        }
        
    }

    private void StartShow(int index)
    {
        ProcessData temp = GenerationHistory[index];

        Debug.Log("Gen: " + temp.GenerationIndex + ", best score: " + temp.BestScore);

        StartSimulationMenu.StartShowcase();
        PopulationManager.StartShowcase(temp);        
    }

    public void ResetList()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateGenerationPanel(ProcessData data, int index)
    {
        GameObject child = Instantiate(GenerationPanelPrefab, transform);
        GenerationPanelScript childGen = child.GetComponent<GenerationPanelScript>();

        childGen.GenerationData = data;
        childGen.SetButtonEvent(StartShow, index);
        
    }
}
