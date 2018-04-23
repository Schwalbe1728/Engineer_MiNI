using DataAcquiringModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EngPlayerCommands;
using CommandGiverModule;
using BindAIToUnityModule;

public class AIInputSource : InputSource, IAcquireData, IGiveCommand
{
    private SensorScript[] Sensors;

    [SerializeField]
    private Transform SensorsSource;

    [SerializeField]
    private IAIUnityBinder AIObject;

    [SerializeField]
    private GameplayScript gameplayScript;

	public bool log;

    public void ActivateSensors()
    {
        if (Sensors != null)
        {
            foreach (SensorScript sensor in Sensors)
            {
                sensor.Activate();
            }
        }
    }

    public void DeativateSensors()
    {
        foreach (SensorScript sensor in Sensors)
        {
            sensor.Deactivate();
        }
    }

    public SensorData GetData()
    {
        float[] distances = new float[Sensors.Length];
        int activeSensors = 0;

        for(int i = 0; i < distances.Length; i++)
        {
            distances[i] = Sensors[i].Distance;
            if (Sensors[i].SensorActive) activeSensors++;
        }

        SensorData result = new SensorData(distances);

        if(gameplayScript != null && result != null)
        {
            result.InsertData("Score", gameplayScript.Score);
            result.InsertData("Game In Progress", (gameplayScript.InProgress) ? 1 : 0);
            result.InsertData("Active Sensors", activeSensors);
            result.InsertData("Velocity", gameplayScript.Velocity());
        }

        if (activeSensors == 0 && gameplayScript.InProgress) Debug.LogWarning("Nieaktywne czujniki!!!");

        return result;
    }

    public void GiveCommand(Command command)
    {
        CommandsList.Add(command);
    }

    public void BindWithCar(GameObject car)
    {
        SensorsSource = car.transform;
        gameplayScript = car.GetComponent<GameplayScript>();
    }

    public void BindWithAI(IAIUnityBinder speciman)
    {
        AIObject = speciman;
        AIObject.SetCommandGiver(this);
        AIObject.SetDataReceiver(this);
    }

    void Start()
    {
        CommandsList = new List<Command>();
        Sensors = SensorsSource.GetComponentsInChildren<SensorScript>();
        ActivateSensors();      
    }
}
