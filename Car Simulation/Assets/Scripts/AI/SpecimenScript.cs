using BindAIToUnityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandGiverModule;
using DataAcquiringModule;
using System;
using NeuralNetwork.Core.Model;
using EngPlayerCommands;
using Assets.Scripts.AI;

public class SpecimenScript : MonoBehaviour, IAIUnityBinder
{
    private static float DeadZoneThreshold = 0.01f;

    private IAcquireData DataReceiver;
    private IGiveCommand CommandGiver;

    private NetworkBase<double> NeuralNetwork;
    private SensorData LastSensorReading;

    public bool GameFinished { get; private set; }

    [SerializeField]
    private bool logEnabled;

    public void SetCommandGiver(IGiveCommand comm)
    {
        CommandGiver = comm;
    }

    public void SetDataReceiver(IAcquireData data)
    {
        DataReceiver = data;
    }

    public void SetNeuralNetwork(NetworkBase<double> network)
    {
        NeuralNetwork = network;
        LastSensorReading = null;
    }

    public int FinalScore()
    {        
        return (LastSensorReading != null)? (int)LastSensorReading.Data("Score") : int.MinValue;
    }

    public void GameStarted()
    {
        GameFinished = false;
    }

    void Awake()
    {
        GameFinished = true;
    }

    void FixedUpdate()
    {        
        if(!GameFinished && DataReceiver != null)
        {
            LastSensorReading = DataReceiver.GetData();
            
            if (LastSensorReading != null && LastSensorReading.Data("Game In Progress") > 0)
            {
                int maxSensor = 0;

                double[] conversionDummy = new double[LastSensorReading.Sensors.Length + 1];
                for(int i = 1; i < conversionDummy.Length; i++)
                {
                    conversionDummy[i] = (double)LastSensorReading.Sensors[i-1];

                    if (conversionDummy[i] >= 1) maxSensor++;
                }

                conversionDummy[0] = LastSensorReading.Data("Velocity") / 60;

                if(logEnabled)
                {
                    string temp =
                        "Velocity: " + conversionDummy[0].ToString("n3") + ", " +
                        "Sensor 0: " + conversionDummy[1].ToString("n3") + ", " +
                        "Sensor 1: " + conversionDummy[2].ToString("n3") + ", " +
                        "Sensor 2: " + conversionDummy[3].ToString("n3") + ", " +
                        "Sensor 3: " + conversionDummy[4].ToString("n3") + ", " +
                        "Sensor 4: " + conversionDummy[5].ToString("n3");

                    Debug.Log(temp);
                }

				var result = NeuralNetwork.Calculate(conversionDummy);
				CalculateNextOrder(result);
            }
            else
            {
                GameFinished = true;
            }
        }        
    }

    private void CalculateNextOrder(double[] sensorReadings)
    {
        double turningDecision = sensorReadings[0];
        double accelerateDecision = sensorReadings[1];

        if(ValueOverThreshold(turningDecision))
        {
            float turnValue = (Mathf.Abs((float)turningDecision) - DeadZoneThreshold) / (1 - DeadZoneThreshold);

            CommandGiver.GiveCommand(
                new SimplifiedCommand(
                    (turningDecision > 0) ? 
                        CommandType.TurnRight : 
                        CommandType.TurnLeft, turnValue));
        }

        if (ValueOverThreshold(accelerateDecision))
        {
            float accelValue = (Mathf.Abs((float)accelerateDecision) - DeadZoneThreshold) / (1 - DeadZoneThreshold);

            CommandGiver.GiveCommand(
                new SimplifiedCommand(
                    (accelerateDecision > 0) ?
                        CommandType.Accelerate :
                        CommandType.Break,
                        accelValue
                    )
                );
        }
    }

    private bool ValueOverThreshold(double val)
    {
        return Math.Abs(val) > DeadZoneThreshold;
    }
}
