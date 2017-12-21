using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManagerScript : MonoBehaviour
{
    [SerializeField]
    private CarsOnSceneManager CarsManager;

    [SerializeField]
    private float MinimumDeactivationDistance;

    private Transform[] Waypoints;
    //private bool[] WaypointPassed;
    private int CurrentWaypoint;
    private float CurrentMaxDistance;

    public float ScoreProgressToWaypoint(Transform car)
    {
        float distance = Vector3.Distance(car.position, Waypoints[CurrentWaypoint].position);
        float max = CurrentMaxDistance;

        if(distance < MinimumDeactivationDistance)
        {
            CheckIfWaypointPassed(true);
        }

        return (max - distance) / max;
    }

    public void Reset()
    {
        CurrentWaypoint = -1;
    }

    void Awake()
    {
        Waypoints = GetComponentsInChildren<Transform>();
        //WaypointPassed = new bool[Waypoints.Length];

        CurrentWaypoint = -1;
        //CheckIfWaypointPassed(true);
    }

    void Update()
    {
        //CheckIfWaypointPassed();
    }

    private void CheckIfWaypointPassed(bool forcePass = false)
    {                
        if(forcePass || CurrentWaypoint < 0 || Vector3.Distance(Waypoints[CurrentWaypoint].position, CarsManager.AveragePositionAndRelax()) < MinimumDeactivationDistance)
        {
            CurrentWaypoint++;
            CurrentWaypoint %= Waypoints.Length;
            PushWaypoints();            
        }
    }

    private void PushWaypoints()
    {
        Transform current = Waypoints[CurrentWaypoint];
        Transform previous = (CurrentWaypoint == 0) ? CarsManager.transform : Waypoints[CurrentWaypoint - 1];
        CurrentMaxDistance = Vector3.Distance(current.position, previous.position);

        //Debug.Log("Waypoint: " + CurrentWaypoint + ", MaxDistance: " + CurrentMaxDistance);
    }
}
