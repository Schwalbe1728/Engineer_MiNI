using NeuralNetwork.Core.Learning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotsManagerScript : MonoBehaviour {

    [SerializeField]
    private PopulationManagerScript PopulationManager;

    [SerializeField]
    private PlotScript BestScorePlot;

    [SerializeField]
    private PlotScript WorstScorePlot;

    [SerializeField]
    private PlotScript AverageScorePlot;

    [SerializeField]
    private PlotScript MedianScorePlot;

    void Awake()
    {
        PopulationManager.OnRoundEnded += OnRoundEnded;
    }

	private void OnRoundEnded(ProcessData generationResults)
    {
        //Debug.Log("OnRoundEnded: PlotsManagerScript");

        if (generationResults != null)
        {
            AddValueToPlot(BestScorePlot, generationResults.BestScore);
            AddValueToPlot(WorstScorePlot, generationResults.WorstScore);
            AddValueToPlot(AverageScorePlot, generationResults.AverageScore);
            AddValueToPlot(MedianScorePlot, generationResults.MedianScore);
        }
    }

    private void AddValueToPlot(PlotScript plot, double val)
    {
        AddValueToPlot(plot, (float)val);
    }

    private void AddValueToPlot(PlotScript plot, float val)
    {
        if(plot != null)
        {
            plot.AddValue(val);
        }
    }
}
