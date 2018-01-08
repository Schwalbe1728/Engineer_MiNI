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

    private float currentDistance;

    private float timer;
    private float updateDelay;

    private bool Work;

    public void Activate()
    {
        Work = true;
        timer = 0f;
    }

    public void Deactivate()
    {
        Work = false;
    }

	// Use this for initialization
	void Start ()
    {
        currentDistance = 1;
        updateDelay = 1.0f / ((RandomizeFrequency)? 
                                    Random.Range(UpdateFrequency-4, UpdateFrequency+4) :
                                    UpdateFrequency
                                );

        Activate();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {        
        if (Work && timer <= 0f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxRange))
            {
                currentDistance = hit.distance / MaxRange;
                //Debug.Log(hit.collider.tag);
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
