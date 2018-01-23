﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisScript : MonoBehaviour {

    [SerializeField]
    private AxisType Axis;

    [SerializeField]
    private PopulationManagerScript PopulationManager;

    [SerializeField]
    [Range(1, 10)]
    private int QuantityOfQuants;

    [SerializeField]
    private GameObject QuantPrefab;

    [SerializeField]
    private MultipleTrendPlotScript Plot;

    private float[] QuantsNormalizedValues;
    private List<QuantScript> QuantsCreated;
    private List<RectTransform> QuantsCreatedRectTransforms;

    void Awake()
    {
        //OnValidate();
        //CreateQuants(QuantityOfQuants);
        QuantsCreated = new List<QuantScript>();
        PopulationManager.OnRoundEnded += ManageQuants;
    }

    void OnValidate()
    {
        //CreateQuants(QuantityOfQuants);
    }
    

    private void ManageQuants(NeuralNetwork.Core.Learning.ProcessData processData)
    {
        float min = 0;
        float max = 0;

        switch(Axis)
        {
            case AxisType.X:

                if(Plot.Count < QuantityOfQuants)
                {
                    if (Plot.Count > 0)
                    {
                        CreateQuants(Plot.Count);
                    }
                }
                else
                {
                    if(QuantsNormalizedValues == null || QuantsNormalizedValues.Length < QuantityOfQuants)
                    {
                        CreateQuants(QuantityOfQuants);
                    }
                }

                min = Plot.MinX;
                max = Plot.MaxX;

                break;

            case AxisType.Y:
                CreateQuants(QuantityOfQuants);
                min = Plot.MinY;
                max = Plot.MaxY;
                break;
        }

        if (QuantsNormalizedValues != null)
        {
            int[] QuantsAxisValues = new int[QuantsNormalizedValues.Length];

            for (int i = 0; i < QuantsNormalizedValues.Length; i++)
            {
                QuantsAxisValues[i] = Mathf.RoundToInt(Mathf.Lerp(min, max, QuantsNormalizedValues[i]));
                if (i < QuantsCreated.Count)
                {
                    QuantsCreated[i].SetText(QuantsAxisValues[i], "n0");

                    float position = 0;

                    switch (Axis)
                    {
                        case AxisType.X:
                            position = Plot.GetXPositionOfValue(QuantsAxisValues[i]);
                            float y = QuantsCreatedRectTransforms[i].anchorMax.y;
                            QuantsCreatedRectTransforms[i].anchorMax = new Vector2(position, y);
                            y = QuantsCreatedRectTransforms[i].anchorMin.y;
                            QuantsCreatedRectTransforms[i].anchorMin = new Vector2(position, y);
                            break;

                        case AxisType.Y:
                            position = Plot.GetYPositionOfValue(QuantsAxisValues[i]);
                            float x = QuantsCreatedRectTransforms[i].anchorMax.x;
                            QuantsCreatedRectTransforms[i].anchorMax = new Vector2(x, position);
                            x = QuantsCreatedRectTransforms[i].anchorMin.x;
                            QuantsCreatedRectTransforms[i].anchorMin = new Vector2(x, position);

                            break;
                    }
                }
            }
        }
    }

    private void CreateQuants(int n)
    {
        QuantsNormalizedValues = new float[n];
        float d = 1.0f / (n - 1);

        for (int i = 0; i < n; i++)
        {
            QuantsNormalizedValues[i] = i * d;

            if (QuantsCreatedRectTransforms == null) QuantsCreatedRectTransforms = new List<RectTransform>();

            if(QuantsCreated != null && QuantsCreated.Count < n)
            {
                QuantsCreated.Add(
                    Instantiate(QuantPrefab, this.transform).GetComponent<QuantScript>()
                    );

                QuantsCreatedRectTransforms.Add
                    (
                        QuantsCreated[QuantsCreated.Count - 1].GetComponent<RectTransform>()
                    );

                //Debug.Log("Quants: " + QuantsCreated.Count);
            }

        }

        QuantsNormalizedValues[n - 1] = 1f;
    }
}

public enum AxisType
{
    X,
    Y
}
