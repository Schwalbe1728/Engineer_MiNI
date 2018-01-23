using NeuralNetwork.Core.Helpers.Gen;
using NeuralNetwork.Core.Helpers.Serializator;
using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Learning.Enums;
using NeuralNetwork.Core.Model;
using NeuralNetwork.Core.Model.Neurons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Troschuetz.Random;
using UnityEngine;
using UnityEngine.UI;

public delegate void RoundEnded(ProcessData processData);

public class PopulationManagerScript : MonoBehaviour
{
    public static KeyCode StopGeneration = KeyCode.R;

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
    private ParentChoosingMethod parentChoosingMethod;

    [SerializeField]
    private NeuronDefinitionsPanelScript neuronDefinitionsPanelScript;

    [SerializeField]
    private MultipleTrendPlotScript plot;

    private bool simulationStarted = false;

    private SpecimenScript[] Specimen;
    private GeneticAlgorithmConfig config;
    private LearningProcess learningProcess;

    private List<int> HiddenLayersSettings;
    private List<Type> NeuronTypesSettings;

    public float sigma;

    public void SaveLearningProcess(string path)
    {
        if(learningProcess.Serialize(path))
        {
            Debug.Log("Zapisano w " + path + " , smochodzików " + learningProcess.PopulationCount);
        }
        else
        {
            Debug.Log("Błąd serializacji!");
        }
    }

    public LearningProcess LoadLearningProcess(string path)
    {
        if (path == null || !File.Exists(path)) return null;

        learningProcess = Serializator.Deserialize(path);

        if(learningProcess == null)
        {
            throw new Exception("Błąd deserializacji!!!");
        }

        config = learningProcess.LearningAlgorithm.Config;
        parentChoosingMethod = config.ParentMethod;
        sigma = (float)config.RandOptions.Sigma;

        PercentToSelect = (float)config.PercentToSelect;
        MutationChance = (float)config.MutationChance;

        List<Type> neuronTypes = new List<Type>();

        foreach(LayerBase<double> neuron in learningProcess.Population[0].Layers)
        {
            neuronTypes.Add(neuron.Neurons[0].GetType());
        }

        Debug.Log(learningProcess.Population[0].Layers[0].Neurons[0]);

        
        neuronDefinitionsPanelScript.
            SetNeuronDefinitions(
                learningProcess.Population[0].LayersCount,
                neuronTypes
            );

        plot.PreviousLearningProcess(learningProcess);
        plot.RestartedSimulation(learningProcess);

        return learningProcess;
    }

    public void SetParentChoosingMethod(ParentChoosingMethod meth)
    {
        parentChoosingMethod = meth;
    }

    public void MakeLearningProcess(int SpecimenCount, float mutChance, float selectPercent, bool overrideLearningProcess = false)
    {
        if (overrideLearningProcess || HiddenLayersSettings == null || NeuronTypesSettings == null)
        {
            neuronDefinitionsPanelScript.GetNeuronDefinitions(out HiddenLayersSettings, out NeuronTypesSettings);
            HiddenLayersSettings.Insert(0, 6);
        }

        PercentToSelect = selectPercent;
        MutationChance = mutChance;
        config.PercentToSelect = selectPercent;
        config.MutationChance = mutChance;

        config.SetParentChoosingMethod(parentChoosingMethod);
        config.RandOptions.Sigma = sigma;

        if (overrideLearningProcess || learningProcess == null)
        {
            learningProcess =
                new LearningProcess(
                    SpecimenCount, config,
                    HiddenLayersSettings,
                    NeuronTypesSettings
                    );            
        }
        else
        {
            Debug.Log("Istniał Learning Process");

            //learningProcess.PopulationCount = Specimen.Length;
        }

        learningProcess.LearningAlgorithm.Config = config;
    }

    public void StartSimulation(float mutChance, float selectPercent, bool configurationStart = false)
    {
        //Debug.Log("Start Simulation - Mutation Chance = " + mutChance + ", Selection Percent = " + selectPercent);        

        simulationStarted = true;
        if(Specimen == null) Specimen = transform.GetComponentsInChildren<SpecimenScript>();

        if (configurationStart)
        {
            MakeLearningProcess(Specimen.Length, mutChance, selectPercent, false);
        }              

        for(int i = 0; i < Specimen.Length; i++)
        {
            Specimen[i].SetNeuralNetwork(learningProcess.Population[i % learningProcess.Population.Length]);
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

    public void StopSimulation(bool stopForGood = false)
    {
        simulationStarted = false;

        if (!stopForGood)
        {
            double[] scores = new double[Specimen.Length];

            for (int i = 0; i < Specimen.Length; i++)
            {
                scores[i] = (double)(Specimen[i].FinalScore());
            }

            learningProcess.Learn(scores);
            int temp = learningProcess.HistoricalData.Count;
            ProcessData tmpData = learningProcess.HistoricalData[temp - 1];

            if (OnRoundEnded != null)
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
        else
        {
            Specimen = null;
            learningProcess = null;
            OnRoundEnded(null);
        }
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
        config.SetParentChoosingMethod(parentChoosingMethod);

        Application.runInBackground = true;
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
