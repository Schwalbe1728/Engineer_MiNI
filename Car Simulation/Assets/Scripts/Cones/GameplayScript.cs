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
    private int currentPenalty;
    private bool GameInProgress;
    private Coroutine coroutine;

	public void EndGame()
    {
        GameInProgress = false;

        Debug.Log("Score: " + Score);

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

        if(OnGameStarted != null)
        {
            OnGameStarted();
        }
    }

    public void SetStartPosition(Transform start)
    {
        StartPosition = start;
    }

    void Start()
    {
        OnGameEnded += CheckIfNewRecord;

        //Restart();        
    }

    void FixedUpdate()
    {
        currentScore += (500 * Vector3.Distance(LastPosition, CarPosition.position) + 1 * Velocity()) * Time.deltaTime;

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
        while(GameInProgress)
        {
            yield return new WaitForSeconds(0.1f);
            currentPenalty++;
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
