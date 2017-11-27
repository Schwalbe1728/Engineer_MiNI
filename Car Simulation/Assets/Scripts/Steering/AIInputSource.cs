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
    //[SerializeField]
    private SensorScript[] Sensors;

    [SerializeField]
    private Transform SensorsSource;

    [SerializeField]
    private IAIUnityBinder AIObject;

    [SerializeField]
    private GameplayScript gameplayScript;

    public SensorData GetData()
    {
        float[] distances = new float[Sensors.Length];

        for(int i = 0; i < distances.Length; i++)
        {
            distances[i] = Sensors[i].Distance;
        }

        SensorData result = new SensorData(distances);

        if(gameplayScript != null)
        {
            result.InsertData("Score", gameplayScript.Score);
            result.InsertData("Game In Progress", (gameplayScript.InProgress) ? 1 : 0);
        }

        return result;
    }

    //TODO : wrzucić to do osobnej biblioteki i interfejsu!!!
    public void GiveCommand(Command command)
    {
        CommandsList.Add(command);
    }

    public void BindWithCar(GameObject car)
    {
        SensorsSource = car.transform;
        gameplayScript = car.GetComponent<GameplayScript>();
    }

    void Awake()
    {
        CommandsList = new List<Command>();

        //Sensors = SensorsSource.GetComponentsInChildren<SensorScript>();

        // wstaw ten obiekt do obiektu AI
        if (AIObject != null)
        {
            AIObject.SetCommandGiver(this);
            AIObject.SetDataReceiver(this);
        }
    }
}
