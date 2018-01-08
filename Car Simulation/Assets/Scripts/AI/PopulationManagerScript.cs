using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using System;
using System.Collections;
using System.Collections.Generic;
using Troschuetz.Random;
using UnityEngine;
using UnityEngine.UI;

public delegate void RoundEnded(ProcessData processData);

public class PopulationManagerScript : MonoBehaviour
{
    public bool SimulationStarted { get { return simulationStarted; } }
    public event RoundEnded OnRoundEnded;

    [SerializeField]
    private float PercentToSelect;

    [SerializeField]
    private float MutationChance;

    [SerializeField]
    private Text GenNumberText;

    [SerializeField]
    private Text BestScoreText;

    [SerializeField]
    private Text WorstScoreText;

    [SerializeField]
    private Text AverageScoreText;

    [SerializeField]
    private Text MedianScoreText;


    [SerializeField]
    private NeuronDefinitionsPanelScript neuronDefinitionsPanelScript;

    private bool simulationStarted = false;

    private SpecimenScript[] Specimen;
    private GeneticAlgorithmConfig config;
    private LearningProcess learningProcess;

    private List<int> HiddenLayersSettings;
    private List<Type> NeuronTypesSettings;

    public float sigma = 0.1f;

    public void StartSimulation(float mutChance, float selectPercent)
    {
        //Debug.Log("Start Simulation - Mutation Chance = " + mutChance + ", Selection Percent = " + selectPercent);

        if(HiddenLayersSettings == null || NeuronTypesSettings == null)
        {
            neuronDefinitionsPanelScript.GetNeuronDefinitions(out HiddenLayersSettings, out NeuronTypesSettings);
            HiddenLayersSettings.Add(2);
            //HiddenLayersSettings.Insert(0, 5);
        }

        simulationStarted = true;
        if(Specimen == null) Specimen = transform.GetComponentsInChildren<SpecimenScript>();

        if (learningProcess == null)
        {
            PercentToSelect = selectPercent;
            MutationChance = mutChance;

            config.PercentToSelect = PercentToSelect;
            config.MutationChance = MutationChance;
            /*
            learningProcess =
                new LearningProcess(Specimen.Length, config,
                    new List<int> { 5, 4, 2 },
                    new List<Type> { typeof(TanHNeuron), typeof(TanHNeuron), /*typeof(TanHNeuron) }
                );
            */
            //learningProcess.NewRandomPopulation(Specimen.Length, new List<int> { 3, 5, 4, 3, 2 }
            //, new List<Type> { typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron) });
            config.RandOptions.Sigma = sigma;
            learningProcess =
                new LearningProcess(
                    Specimen.Length, config,
                    HiddenLayersSettings,
                    NeuronTypesSettings
                    );

            learningProcess.LearningAlgorithm.Config = config;
        }        

        for(int i = 0; i < Specimen.Length; i++)
        {
            Specimen[i].SetNeuralNetwork(learningProcess.Population[i]);
        }

        foreach(SpecimenScript spec in Specimen)
        {
            spec.GameStarted();
        }
    }

    public void RoundEnded()
    {
        StopSimulation();
        StartSimulation(MutationChance, PercentToSelect);
    }

    public void StopSimulation()
    {
        simulationStarted = false;

        double[] scores = new double[Specimen.Length];

        for(int i = 0; i < Specimen.Length; i++)
        {
            scores[i] = (double)(Specimen[i].FinalScore());
        }

        learningProcess.Learn(scores);
        int temp = learningProcess.HistoricalData.Count;
        ProcessData tmpData = learningProcess.HistoricalData[temp - 1];

        if(OnRoundEnded != null)
        {
            OnRoundEnded(tmpData);
        }

        Debug.Log(
            "Generation " + tmpData.GenerationIndex + 
            ", Avg: " + tmpData.AverageScore.ToString("n2") + 
            ", Med: " + tmpData.MedianScore + 
            ", Best: " + tmpData.BestScore + 
            ", Worst: " + tmpData.WorstScore);
    }

    public List<SpecimenScript> GetActiveCars()
    {
        List<SpecimenScript> result = new List<SpecimenScript>();

        foreach(SpecimenScript speciman in Specimen)
        {
            if(!speciman.GameFinished)
            {
                result.Add(speciman);
            }
        }

        return result;
    }

    void Awake()
    {
        config = new GeneticAlgorithmConfig();
        config.RandOptions = new RandomizerOptions(-1, 1);
        config.PercentToSelect = PercentToSelect;
        config.MutationChance = MutationChance;
        //config.ParentChances = Chances;
        config.SetParentChoosingMethod(NeuralNetwork.Core.Learning.Enums.ParentChoosingMethod.PositionExponential);
    }

    void Update()
    {
        if (Specimen != null)
        {
            GenNumberText.text = " ";

            float Min = int.MaxValue;
            float Max = int.MinValue;
            float Med;
            float Sum = 0;

            List<float> results = new List<float>();

            foreach (SpecimenScript speciman in Specimen)
            {
                float score = speciman.FinalScore();
                Sum += score;
                results.Add(score);
            }

            results.Sort();
            Min = results[0];
            Max = results[results.Count - 1];
            Med =
                (results.Count % 2 == 0 && results.Count > 1) ?
                    (results[results.Count / 2] + results[results.Count / 2 - 1]) / 2 :
                    results[results.Count / 2];

            WorstScoreText.text = Min.ToString("n0");
            BestScoreText.text = Max.ToString("n0");
            AverageScoreText.text = (Sum / Specimen.Length).ToString("n1");
            MedianScoreText.text = Med.ToString("n1");
        }
    }

    private int[] Chances(int n)
    {        
        int[] result = new int[n];

        for (int i = 0; i < n; i++)
        {
            result[i] = (3 * n - 2 * i);//*(n-i);
        }
        return result;
        
        /*
        float[] scores = new float[Specimen.Length];
        float max = -300;

        for (int i = 0; i < Specimen.Length; i++)
        {
            scores[i] = Specimen[i].FinalScore();
            if (max < scores[i]) max = scores[i];
        }

        int[] result = new int[n];

        for(int i = 0; i < n; i++)
        {
            result[i] = Mathf.Max(1, Mathf.RoundToInt(20 * scores[i] / max));
        }

        return result;
        */
    }
}
