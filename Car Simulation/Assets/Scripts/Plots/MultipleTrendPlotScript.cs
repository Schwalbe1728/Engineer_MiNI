using NeuralNetwork.Core.Learning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MultipleTrendPlotScript : MonoBehaviour
{
    [SerializeField]
    private PopulationManagerScript PopulationManager;

    [SerializeField]
    private RectTransform plotArea;

    [SerializeField]
    private UILineRenderer XAxisRenderer;

    [SerializeField]
    private UILineRenderer YAxisRenderer;

    [SerializeField]
    private float Expansion;

    private Plot BestScorePlot;
    private Plot WorstScorePlot;
    private Plot AverageScorePlot;
    private Plot MedianScorePlot;

    private float MaxY;
    private float MinY;

    private List<Plot> PlotsArray;

    [SerializeField]
    private UILineRenderer[] PlotLinesRenderersArray;

    [SerializeField]
    private Color[] PlotsColors;

    void Awake()
    {
        PlotsArray = new List<Plot>();
        OnValidate();

        AddPlotToArray(BestScorePlot);
        AddPlotToArray(WorstScorePlot);
        AddPlotToArray(AverageScorePlot);
        AddPlotToArray(MedianScorePlot);

        PopulationManager.OnRoundEnded += UpdatePlots;        
    }

    void OnValidate()
    {
        BestScorePlot = new Plot(Expansion);
        WorstScorePlot = new Plot(Expansion);
        AverageScorePlot = new Plot(Expansion);
        MedianScorePlot = new Plot(Expansion);

        if (PlotLinesRenderersArray != null)
        {
            int i = 0;

            foreach (UILineRenderer lineRend in PlotLinesRenderersArray)
            {
                if (lineRend != null)
                {
                    lineRend.color = PlotsColors[i++];
                }
            }
        }
    }

    private void UpdatePlots(ProcessData generationResults)
    {
        Debug.Log("UpdatePlots");

        AddValueToPlot(BestScorePlot, generationResults.BestScore);
        AddValueToPlot(WorstScorePlot, generationResults.WorstScore);
        AddValueToPlot(AverageScorePlot, generationResults.AverageScore);
        AddValueToPlot(MedianScorePlot, generationResults.MedianScore);

        int i = 0;

        foreach(Plot plot in PlotsArray)
        {
            PlotLinesRenderersArray[i].m_points = 
                plot.GetPlotPoints(plotArea.rect.width, plotArea.rect.height, true, MinY, MaxY).ToArray();

            PlotLinesRenderersArray[i].SetVerticesDirty();

            i++;
        }

        XAxisRenderer.m_points[0].y = GetYPositionOfValue(0);
        XAxisRenderer.m_points[1].y = GetYPositionOfValue(0);
        XAxisRenderer.SetVerticesDirty();
    }

    private void AddValueToPlot(Plot plot, double val)
    {
        AddValueToPlot(plot, (float)val);
    }

    private void AddValueToPlot(Plot plot, float val)
    {
        if (plot != null)
        {
            plot.Add(val);

            if (val > MaxY) { MaxY = val; }
            if (val < MinY) { MinY = val; }            
        }
    }

    private void AddPlotToArray(Plot plot)
    {
        if (plot != null)
        {
            PlotsArray.Add(plot);
        }
    }

    public float GetYPositionOfValue(float val)
    {
        float dY = MaxY - MinY + 2 * Expansion;

        return (Expansion + val - MinY) / dY;
    }

}
