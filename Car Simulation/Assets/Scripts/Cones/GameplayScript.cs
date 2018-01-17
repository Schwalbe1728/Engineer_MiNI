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

    //private static int AUTOFAIL_SCORE = -250;    

    [SerializeField]
    private Transform CarPosition;

    [SerializeField]
    private Transform StartPosition;

    private Vector3 LastWaypointPosition;
    private Vector3 LastPosition;
    private HashSet<uint> VisitedWaypoints;
    private Vector3 Normal;

    /*
    [SerializeField]
    private Text ScoreValueText;
    */
    [SerializeField]
    private Rigidbody CarRigidbody;
    /*
    [SerializeField]
    private Text VelocityValueText;
    */

    private float currentScore;
    //private float currentPenalty;
    private bool GameInProgress;
    private Coroutine coroutine;

	public void EndGame()
    {
        GameInProgress = false;

        //Debug.Log("Score: " + Score);

        if(OnGameEnded != null)
        {
            OnGameEnded(Score);
        }

        //Restart();
    }

    public bool InProgress
    {
        get { return GameInProgress; }
    }

    public int Score
    {
        get
        {
            return Mathf.RoundToInt(currentScore/* - currentPenalty*/);
        }
    }

    public void Restart()
    {
        GameInProgress = true;

        StopAllCoroutines();
        currentScore = 0;
        //currentPenalty = 0;
        CarPosition.position = StartPosition.position;
        CarPosition.rotation = StartPosition.rotation;
        LastPosition = StartPosition.position;
        LastWaypointPosition = StartPosition.position;

        Normal = gameObject.transform.forward;
        Normal.Normalize();

        if (VisitedWaypoints == null) VisitedWaypoints = new HashSet<uint>();
        ClearWaypointIndexes();

        //coroutine = StartCoroutine(PenalizeTime());
        coroutine = StartCoroutine(CheckIfTimeout());

        //WaypointManager.Reset();

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
        //Restart();        
    }

    void FixedUpdate()
    {
        float lastToCar = Vector3.Distance(LastPosition, CarPosition.position);
        float waypointToCar = Vector3.Distance(LastWaypointPosition, CarPosition.position);
        float waypointToLast = Vector3.Distance(LastWaypointPosition, LastPosition);

        Vector3 waypointToCarVector = CarPosition.position - LastWaypointPosition;
        float direction = Vector3.Dot(waypointToCarVector, Normal);

        //currentScore +=
        float inc = (100 * Mathf.Sign(direction) * (lastToCar + (waypointToCar - waypointToLast))
             /*- 100 * (1-WaypointManager.ScoreProgressToWaypoint(CarPosition))*/
             //+ 50 * (startToCar - startToLast)
             //+ startToCar * ((startToCar > startToLast)? 1 : -0.5f )             
             + 0.005f * Velocity()) * Time.fixedDeltaTime;

        if (inc < 0)
        {
            //Debug.Log("Zmniejsza się!: " + inc.ToString("n3"));
        }
        else
        {
            currentScore += inc;
        }

        LastPosition = CarPosition.position;

        /*if(ScoreValueText != null)
        {
            ScoreValueText.text = Score.ToString();
        }
        */
        /*
        if(VelocityValueText != null)
        {
            VelocityValueText.text = Mathf.RoundToInt(Velocity()).ToString();
        }
        */
    }

    public float Velocity()
    {
        return CarRigidbody.velocity.magnitude * 3.6f;
    }

    /*
    private IEnumerator PenalizeTime()
    {
        float penaltyPerSec = 25.0f;

        while(GameInProgress)
        {
            yield return new WaitForSeconds( 1 / penaltyPerSec);

            if (Time.deltaTime < 1 / penaltyPerSec)
            {
                currentPenalty++;
            }
            else
            {
                currentPenalty += penaltyPerSec * Time.deltaTime;
            }
            //currentPenalty += currentPenalty / 48;

            if(Score < AUTOFAIL_SCORE)
            {
                EndGame();
            }
        }
    }*/

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
        /*
        if( score > PlayerPrefs.GetFloat("Highest Score", 0) )
        {
            PlayerPrefs.SetFloat("Highest Score", score);
            Debug.Log("New record!: " + score);
        }
        */
    }
}
