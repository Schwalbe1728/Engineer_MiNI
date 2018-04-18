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
	public bool Log { get; set; }

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
                //Debug.Log("Score: " + LastSensorReading.Data("Score"));
                int maxSensor = 0;

                //float[] decisionValues = NeuralNetwork.Calculate(LastSensorReading.Sensors);
                double[] reeeee = new double[LastSensorReading.Sensors.Length + 1];
                for(int i = 1; i < reeeee.Length; i++)
                {
                    reeeee[i] = (double)LastSensorReading.Sensors[i-1];

                    if (reeeee[i] >= 1) maxSensor++;
                }

                reeeee[0] = LastSensorReading.Data("Velocity") / 60;

                //reeeee[reeeee.Length - 1] = (double)LastSensorReading.Data("Score");
                /*
                if (UnityEngine.Random.Range(0f, 1f) < 0.01f || maxSensor == reeeee.Length)
                {
                    Debug.Log("Sensors: Active = " + LastSensorReading.Data("Active Sensors").ToString("n0") + " " +
                        reeeee[0].ToString("n3") + " " +
                        reeeee[1].ToString("n3") + " " +
                        reeeee[2].ToString("n3") + " " +
                        reeeee[3].ToString("n3") + " " +
                        reeeee[4].ToString("n3")
                        );
                }

                if (maxSensor == reeeee.Length) throw new Exception("Czujniki się zjebały?!!");
                */
				var result = NeuralNetwork.Calculate(reeeee);
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
        //Debug.Log("Interpretuj: " + sensorReadings[0].ToString("n2") + " " + sensorReadings[1].ToString("n2"));    

        double turningDecision = sensorReadings[0];
        double accelerateDecision = sensorReadings[1];

        if(ValueOverThreshold(turningDecision))
        {
            /*CommandGiver.GiveCommand(
                new Turn(
                            (turningDecision > 0)? 
                                TurnDirection.Right :
                                TurnDirection.Left
                                ));
                                */
            float turnValue = (Mathf.Abs((float)turningDecision) - DeadZoneThreshold) / (1 - DeadZoneThreshold);

            CommandGiver.GiveCommand(
                new SimplifiedCommand(
                    (turningDecision > 0) ? 
                        CommandType.TurnRight : 
                        CommandType.TurnLeft, turnValue));

            //Debug.Log("Turn");
        }

        if (ValueOverThreshold(accelerateDecision))
        {/*
            if(accelerateDecision > 0)
            {
                CommandGiver.GiveCommand(new Accelerate());
            }
            else
            {
                CommandGiver.GiveCommand(new Break());
            }*/

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
