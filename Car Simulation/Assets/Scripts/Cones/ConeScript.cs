using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeScript : MonoBehaviour
{
    private Transform CarPosition;
    private GameplayScript gameplayScript;

    void Awake()
    {
        CarPosition = GameObject.Find("Car").GetComponent<Transform>();
        gameplayScript = GameObject.Find("Gameplay Manager").GetComponent<GameplayScript>();
    }
	
    void OnCollisionEnter(Collision collision)
    {
        gameplayScript.EndGame();
    }
}
