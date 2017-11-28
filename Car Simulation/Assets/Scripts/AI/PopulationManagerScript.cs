using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Learning;
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

        if(learningProcess == null) learningProcess = new LearningProcess(Specimen.Length, config);

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
    }

    void Awake()
    {
        config = new GeneticAlgorithmConfig();
        config.RandOptions = new RandomizerOptions(-1, 1);
        config.PercentToSelect = 0.5;
        config.MutationChance = 0.1;
        config.ParentChances = Chances;
    }

    private int[] Chances(int n)
    {
        int[] result = new int[n];

        for (int i = 0; i < n; i++)
        {
            result[i] = n - i;
        }
        return result;
    }
}
