using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorScript : MonoBehaviour {

    public float Distance { get { return currentDistance; } }

    [SerializeField]
    private float MaxRange;

    private float currentDistance;

	// Use this for initialization
	void Start ()
    {
        currentDistance = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        RaycastHit hit;

	    if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxRange))
        {
            currentDistance = hit.distance / MaxRange;            
        }
        else
        {
            currentDistance = 1;
        }
	}
}
