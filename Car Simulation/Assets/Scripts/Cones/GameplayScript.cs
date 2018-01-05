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

    private static int AUTOFAIL_SCORE = -250;
    private WaypointManagerScript WaypointManager;

    [SerializeField]
    private Transform CarPosition;

    [SerializeField]
    private Transform StartPosition;

    private Vector3 LastPosition;
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
    private float currentPenalty;
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
            return Mathf.RoundToInt(currentScore - currentPenalty);
        }
    }

    public void Restart()
    {
        GameInProgress = true;

        StopAllCoroutines();
        currentScore = 0;
        currentPenalty = 0;
        CarPosition.position = StartPosition.position;
        CarPosition.rotation = StartPosition.rotation;
        LastPosition = StartPosition.position;
        GameInProgress = true;
        coroutine = StartCoroutine(PenalizeTime());

        //WaypointManager.Reset();

        if(OnGameStarted != null)
        {
            OnGameStarted();
        }
    }

    public void SetStartPosition(Transform start)
    {
        StartPosition = start;
    }

    void Awake()
    {
        OnGameEnded += CheckIfNewRecord;

        WaypointManager = GameObject.Find("Waypoints").GetComponent<WaypointManagerScript>();
        //Restart();        
    }

    void FixedUpdate()
    {
        float lastToCar = Vector3.Distance(LastPosition, CarPosition.position);
        float startToCar = Vector3.Distance(StartPosition.position, CarPosition.position);
        float startToLast = Vector3.Distance(StartPosition.position, LastPosition);

        currentScore +=
            (300 * (lastToCar / 2 + startToCar - startToLast )
             /*- 100 * (1-WaypointManager.ScoreProgressToWaypoint(CarPosition))*/
             //+ 50 * (startToCar - startToLast)
             //+ startToCar * ((startToCar > startToLast)? 1 : -0.5f )             
             + 0.1f * Velocity()) * Time.deltaTime;

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

    private float Velocity()
    {
        return CarRigidbody.velocity.magnitude * 3.6f;
    }

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
