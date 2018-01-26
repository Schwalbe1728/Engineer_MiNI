using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationPanelScript : MonoBehaviour
{
    [SerializeField]
    private Text GenNumberText;

    [SerializeField]
    private Text BestScoreText;

    private ProcessData GenData;

    public ProcessData GenerationData
    {        
        set
        {
            GenData = value;
            GenNumberText.text = GenData.GenerationIndex.ToString();
            BestScoreText.text = GenData.BestScore.ToString("n0");
        }
    }    
	
    public NetworkBase<double> GetBestSpeciman()
    {
        return GenData.BestSpecimen;
    }

    public void SetButtonEvent(System.Action<int> call, int index)
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        button.onClick.AddListener(delegate { call(index); });
    }
}
