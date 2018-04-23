using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void GameEnded(int finalScore);
public delegate void GameStarted();

public class GameplayScript : MonoBehaviour
{
    public event GameEnded OnGameEnded;
    public event GameStarted OnGameStarted;
    
    [SerializeField]
    private Transform CarPosition;

    [SerializeField]
    private Transform StartPosition;

    private Vector3 LastWaypointPosition;
    private Vector3 LastPosition;
    private HashSet<uint> VisitedWaypoints;
    private Vector3 Normal;

    [SerializeField]
    private Rigidbody CarRigidbody;

    private float currentScore;
    
    private bool GameInProgress;
    private Coroutine coroutine;

	public void EndGame()
    {
        GameInProgress = false;

        if(OnGameEnded != null)
        {
            OnGameEnded(Score);
        }        
    }

    public bool InProgress
    {
        get { return GameInProgress; }
    }

    public int Score
    {
        get
        {
            return Mathf.RoundToInt(currentScore);
        }
    }

    public void Restart(bool checkForTimeout)
    {
        GameInProgress = true;

        StopAllCoroutines();
        currentScore = 0;
        CarPosition.position = StartPosition.position;
        CarPosition.rotation = StartPosition.rotation;
        LastPosition = StartPosition.position;
        LastWaypointPosition = StartPosition.position;

        Normal = gameObject.transform.forward;
        Normal.Normalize();

        if (VisitedWaypoints == null) VisitedWaypoints = new HashSet<uint>();
        ClearWaypointIndexes();

        coroutine = (checkForTimeout)? StartCoroutine(CheckIfTimeout()) : null;

        if (OnGameStarted != null)
        {
            OnGameStarted();
        }
    }

    public void SetStartPosition(Transform start)
    {
        StartPosition = start;
    }

    public void SetWaypointPosition(Vector3 pos)
    {
        LastWaypointPosition = pos;
    }

    public void IncreaseScore(float val)
    {
        currentScore += val;
    }

    public bool AddWaypointIndex(uint id)
    {        
        bool result = !VisitedWaypoints.Contains(id);

        if(result)
        {
            VisitedWaypoints.Add(id);
        }

        return result;
    }

    public void ClearWaypointIndexes()
    {
        VisitedWaypoints.Clear();
    }

    public void SetNormalToWaypoint(Vector3 norm)
    {
        Normal = norm;
    }

    void Awake()
    {
        OnGameEnded += CheckIfNewRecord;
    }

    void Update()
    {
        if(Input.GetKeyDown(PopulationManagerScript.StopGeneration))
        {
            EndGame();
        }
    }

    void FixedUpdate()
    {
        float lastToCar = Vector3.Distance(LastPosition, CarPosition.position);
        float waypointToCar = Vector3.Distance(LastWaypointPosition, CarPosition.position);
        float waypointToLast = Vector3.Distance(LastWaypointPosition, LastPosition);

        Vector3 waypointToCarVector = CarPosition.position - LastWaypointPosition;
        float direction = Vector3.Dot(waypointToCarVector, Normal);

        float inc = 
            (100 * Mathf.Sign(direction) * (lastToCar + (waypointToCar - waypointToLast))             
             + 0.005f * Velocity()) * Time.fixedDeltaTime;

        if (inc < 0)
        {
        }
        else
        {
            currentScore += inc;
        }

        LastPosition = CarPosition.position;
    }

    public float Velocity()
    {
        return CarRigidbody.velocity.magnitude * 3.6f;
    }
    
    private IEnumerator CheckIfTimeout()
    {
        float lastScore;

        do
        {
            lastScore = Score;
            yield return new WaitForSeconds(5);
        }
        while (lastScore < Score);

        EndGame();
    }

    private void CheckIfNewRecord(int score)
    {
        
    }
}
