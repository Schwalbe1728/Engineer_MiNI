using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class PlotScript : MonoBehaviour
{
    [SerializeField]
    private Plot plotObject;

    [SerializeField]
    private RectTransform plotArea;

    [SerializeField]
    private UILineRenderer XAxisRenderer;

    [SerializeField]
    private UILineRenderer YAxisRenderer;

    [SerializeField]
    private UILineRenderer PlotLineRenderer;


    public void AddValue(float val)
    {
        plotObject.Add(val);
        UpdatePlotArea();
    }

    void Awake()
    {
        //plotObject = new Plot();
        plotArea = GetComponent<RectTransform>();
        UpdatePlotArea();
    }

    private void UpdatePlotArea()
    {
        List<Vector2> plotPoints = plotObject.GetPlotPoints(plotArea.rect.width, plotArea.rect.height);

        XAxisRenderer.m_points[0].y = plotObject.GetYPositionOfValue(0);
        XAxisRenderer.m_points[1].y = plotObject.GetYPositionOfValue(0);

        // wyczyść listę UILineRenderera
        // wstaw plotPoints jako punkty

        PlotLineRenderer.m_points = plotPoints.ToArray();

        PlotLineRenderer.SetVerticesDirty();
        XAxisRenderer.SetVerticesDirty();
    }
}

[System.Serializable]
public class Plot
{
    [SerializeField]
    private float YExpansion;

    [SerializeField]
    private float XExpansion;

    public float MaxY { get; private set; }
    public float MinY { get; private set; }

    private List<float> Values;

    public int Count { get { return (Values != null) ? Values.Count : -1; } }

    public Plot(float Expansion = 10f)
    {
        //YExpansion = Expansion;
        //XExpansion = Expansion;
        MaxY = 0;
        MinY = 0;

        Values = new List<float>();
    }

    public void Add(float val)
    {
        Values.Add(val);

        if (val > MaxY) MaxY = val;
        if (val < MinY) MinY = val;
    }

    public List<Vector2> GetPlotPoints(float width, float height, int n = int.MaxValue, bool given = false, float min = 0, float max = 0)
    {
        List<Vector2> result = new List<Vector2>();

        float Max = (given)? max : MaxY;
        float Min = (given) ? min : MinY;        

        if (Values == null) Values = new List<float>();

        int MinIndexToReturn = (Values.Count - n > 0) ? Values.Count - n : 0;

        if (Values.Count > 0)
        {
            float dY = Max - Min + 2 * YExpansion;
            float dX = (width - 2 * XExpansion) / (Values.Count - MinIndexToReturn - 1);

            float currX = XExpansion;

            foreach(float val in Values.GetLastNElements(n))
            {
                float currY = (YExpansion + val - Min) / dY;

                result.Add(new Vector2(currX / width, currY));
                currX += dX;
            }
        }

        return result;
    }

    public List<Vector2> GetPlotValues()
    {
        List<Vector2> result = new List<Vector2>();
        int i = 0;

        foreach(float val in Values)
        {
            result.Add(new Vector2(i++, val));
        }

        return result;
    }

    public float GetYPositionOfValue(float val)
    {
        float dY = MaxY - MinY + 2 * YExpansion;

        return (YExpansion + val - MinY) / dY;
    }
}