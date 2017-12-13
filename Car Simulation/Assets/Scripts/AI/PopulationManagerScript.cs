using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManagerScript : MonoBehaviour
{
    public bool SimulationStarted { get { return simulationStarted; } }

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

    private bool simulationStarted = false;

    private SpecimenScript[] Specimen;
    private GeneticAlgorithmConfig config;
    private LearningProcess learningProcess;

    public void StartSimulation(float mutChance, float selectPercent)
    {
        //Debug.Log("Start Simulation - Mutation Chance = " + mutChance + ", Selection Percent = " + selectPercent);

        simulationStarted = true;
        if(Specimen == null) Specimen = transform.GetComponentsInChildren<SpecimenScript>();

        if (learningProcess == null)
        {
            PercentToSelect = selectPercent;
            MutationChance = mutChance;

            config.PercentToSelect = PercentToSelect;
            config.MutationChance = MutationChance;

            learningProcess =
                new LearningProcess(Specimen.Length, config,
                    new List<int> { 3, 5, 2 },
                    new List<Type> { typeof(TanHNeuron), typeof(TanHNeuron)/*, typeof(TanHNeuron) */}
                );
            //learningProcess.NewRandomPopulation(Specimen.Length, new List<int> { 3, 5, 4, 3, 2 }
            //, new List<Type> { typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron) });
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
        config.ParentChances = Chances;
    }

    void Update()
    {
        if (Specimen != null)
        {
            GenNumberText.text = " ";

            float Min = int.MaxValue;
            float Max = int.MinValue;
            float Sum = 0;

            foreach (SpecimenScript speciman in Specimen)
            {
                float score = speciman.FinalScore();

                Sum += score;

                if (score > Max)
                {
                    Max = score;
                }

                if (score < Min)
                {
                    Min = score;
                }
            }

            WorstScoreText.text = Min.ToString("n0");
            BestScoreText.text = Max.ToString("n0");
            AverageScoreText.text = (Sum / Specimen.Length).ToString("n1");
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
