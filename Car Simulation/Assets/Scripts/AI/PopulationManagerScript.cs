using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManagerScript : MonoBehaviour
{
    public bool SimulationStarted { get { return simulationStarted; } }

    private bool simulationStarted = false;

    private SpecimenScript[] Specimen;
    private GeneticAlgorithmConfig config;
    private LearningProcess learningProcess;

    public void StartSimulation()
    {
        simulationStarted = true;
        if(Specimen == null) Specimen = transform.GetComponentsInChildren<SpecimenScript>();

        if (learningProcess == null)
        {
            learningProcess = new LearningProcess(Specimen.Length, config);
            learningProcess.NewRandomPopulation(Specimen.Length, new List<int> { 3, 5, 4, 3, 2 }
            , new List<Type> { typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron), typeof(IdentityNeuron) });
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
        StartSimulation();
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

    void Awake()
    {
        config = new GeneticAlgorithmConfig();
        config.RandOptions = new RandomizerOptions(-1, 1);
        config.PercentToSelect = 0.4;
        config.MutationChance = 0.05;
        config.ParentChances = Chances;
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
        }*/

        return result;
    }
}
