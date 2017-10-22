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

    [SerializeField]
    private Text ScoreValueText;

    private float currentScore;
    private int currentPenalty;
    private bool GameInProgress;
    private Coroutine coroutine;

	public void EndGame()
    {
        if(OnGameEnded != null)
        {
            OnGameEnded(Score);
        }

        Restart();
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

    void Start()
    {
        OnGameEnded += CheckIfNewRecord;

        Restart();        
    }

    void FixedUpdate()
    {
        currentScore += 1000 * Vector3.Distance(LastPosition, CarPosition.position) * Time.deltaTime;

        LastPosition = CarPosition.position;

        if(ScoreValueText != null)
        {
            ScoreValueText.text = Score.ToString();
        }
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
        if( score > PlayerPrefs.GetFloat("Highest Score", 0) )
        {
            PlayerPrefs.SetFloat("Highest Score", score);
            Debug.Log("New record!: " + score);
        }
    }
}
