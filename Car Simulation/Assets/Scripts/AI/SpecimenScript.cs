using BindAIToUnityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandGiverModule;
using DataAcquiringModule;
using System;
using NeuralNetwork.Core.Model;
using EngPlayerCommands;

public class SpecimenScript : MonoBehaviour, IAIUnityBinder
{
    private static float DeadZoneThreshold = 0.1f;

    private IAcquireData DataReceiver;
    private IGiveCommand CommandGiver;

    private NetworkBase<double> NeuralNetwork;
    private SensorData LastSensorReading;

    private bool GameFinished;

    public void SetCommandGiver(IGiveCommand comm)
    {
        //throw new NotImplementedException();
        CommandGiver = comm;
    }

    public void SetDataReceiver(IAcquireData data)
    {
        //throw new NotImplementedException();
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
            //Debug.Log("Wybieraj");

            LastSensorReading = DataReceiver.GetData();
            
            if (LastSensorReading != null && LastSensorReading.Data("Game In Progress") > 0)
            {
                //float[] decisionValues = NeuralNetwork.Calculate(LastSensorReading.Sensors);
                double[] reeeee = new double[LastSensorReading.Sensors.Length];
                for(int i = 0; i < reeeee.Length; i++)
                {
                    reeeee[i] = (double)LastSensorReading.Sensors[i];
                }

                CalculateNextOrder(NeuralNetwork.Calculate(reeeee));
            }
            else
            {
                GameFinished = true;
            }
        }        
    }

    private void CalculateNextOrder(double[] sensorReadings)
    {
        //Debug.Log("Interpretuj: " + sensorReadings[0].ToString("n2") + " " + sensorReadings[1].ToString("n2"));

        double turningDecision = sensorReadings[0];
        double accelerateDecision = sensorReadings[1];

        if(ValueOverThreshold(turningDecision))
        {            
            CommandGiver.GiveCommand(
                new Turn(
                            (turningDecision > 0)? 
                                TurnDirection.Right :
                                TurnDirection.Left
                                ));                       
        }

        if (ValueOverThreshold(accelerateDecision))
        {
            if(accelerateDecision > 0)
            {
                CommandGiver.GiveCommand(new Accelerate());
            }
            else
            {
                CommandGiver.GiveCommand(new Break());
            }
        }
    }

    private bool ValueOverThreshold(double val)
    {
        return Math.Abs(val) > DeadZoneThreshold;
    }
}
