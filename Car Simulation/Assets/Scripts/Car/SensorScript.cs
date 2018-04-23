using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour {

    public float Distance { get { return currentDistance; } }

    [SerializeField]
    private float MaxRange;

    [SerializeField]
    [Range(5, 60)]
    private int UpdateFrequency = 30;

    [SerializeField]
    private bool RandomizeFrequency;

    [SerializeField]    
    private float currentDistance;

    private float timer;
    private float updateDelay;

    private bool Work;

    public bool SensorActive { get { return Work; } }

    public void Activate()
    {
        Work = true;
        timer = 0f;
    }

    public void Deactivate()
    {        
        Work = false;
    }

	void Start ()
    {
        currentDistance = 1;
        updateDelay = 1.0f / ((RandomizeFrequency)? 
                                    Random.Range(UpdateFrequency-4, UpdateFrequency+4) :
                                    UpdateFrequency
                                );

        Activate();
	}
	
	void FixedUpdate ()
    {        
        if (Work && timer <= 0f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxRange))
            {
                currentDistance = hit.distance / MaxRange;
            }
            else
            {
                currentDistance = 1;
            }

            timer += updateDelay;
        }

        timer -= Time.fixedDeltaTime;
	}
}
