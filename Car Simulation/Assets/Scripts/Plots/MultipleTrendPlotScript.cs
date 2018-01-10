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

    [SerializeField]
    [Range(10, 100000)]
    private int MaxPointsToShow = 10;

    private Plot BestScorePlot;
    private Plot WorstScorePlot;
    private Plot AverageScorePlot;
    private Plot MedianScorePlot;

    public float MaxY { get; private set; }
    public float MinY { get; private set; }
    public float MaxX { get { return Count - 1; } }
    public float MinX { get { return (Count > 0) ? ((Count - MaxPointsToShow > 0)? Count - MaxPointsToShow : 0  ) : -1; } }

    public int Count { get { return BestScorePlot.Count; } }


    private List<Plot> PlotsArray;

    [SerializeField]
    private UILineRenderer[] PlotLinesRenderersArray;

    [SerializeField]
    private Color[] PlotsColors;

    public void RestartedSimulation()
    {
        MaxY = 0;
        MinY = 0;

        foreach(Plot plot in PlotsArray)
        {
            plot.Clear();
        }

        UpdatePlots(null);
    }

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
        //Debug.Log("UpdatePlots");

        if (generationResults != null)
        {
            AddValueToPlot(BestScorePlot, generationResults.BestScore);
            AddValueToPlot(WorstScorePlot, generationResults.WorstScore);
            AddValueToPlot(AverageScorePlot, generationResults.AverageScore);
            AddValueToPlot(MedianScorePlot, generationResults.MedianScore);
        }

        int i = 0;

        foreach(Plot plot in PlotsArray)
        {
            PlotLinesRenderersArray[i].m_points = 
                plot.GetPlotPoints(plotArea.rect.width, plotArea.rect.height, MaxPointsToShow, true, MinY-Expansion, MaxY+Expansion).
                ToArray();

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

    public float GetXPositionOfValue(float val)
    {
        float result = 0;

        if (BestScorePlot != null && BestScorePlot.Count > 0)
        {            
            float max = BestScorePlot.Count - 1;
            float min = Mathf.Max(BestScorePlot.Count - MaxPointsToShow, 0);

            //result = Mathf.Max(0, Mathf.Min(1f, val / max));                                    
            //result = Mathf.Max(0, Mathf.Min(1f, (val - (max - MaxPointsToShow + 1)) / (MaxPointsToShow - 1)));
            result = Mathf.Max(0, Mathf.Min(1f, (val - min) / (max - min)));
        }

        return result;
    }    
}

public static class ListExtension
{
    public static List<T> GetLastNElements<T>(this List<T> list, int n)
    {
        List<T> result = null;

        if(n > list.Count)
        {
            result = list;
        }
        else
        {
            result = list.GetRange(list.Count - n, n);
        }

        return result;
    }
    /*
    public static List<float> GetLastNElements(this List<float> list, int n)
    {
        List<float> result = null;

        if (n > list.Count)
        {
            result = list;
        }
        else
        {
            result = list.GetRange(list.Count - n, n);
        }

        return result;
    }*/
}
