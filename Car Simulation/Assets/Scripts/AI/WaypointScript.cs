using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour {

    [SerializeField]
    private float BonusPointsForCrossing;

    [SerializeField]
    private uint WaypointID;

    [SerializeField]
    private bool IsFinishLine;

    private Dictionary<GameplayScript, int> rigidbodies;

    [SerializeField]
    private Vector3 Normal;

    void Awake()
    {
        rigidbodies = new Dictionary<GameplayScript, int>();

        Normal = transform.forward;
        Normal.Normalize();
    }

    void Update()
    {
        List<GameplayScript> toDelete = new List<GameplayScript>(rigidbodies.Keys);

        for(int i = 0; i < toDelete.Count; i++)
        {
            if(!toDelete[i].InProgress)
            {
                rigidbodies.Remove(toDelete[i]);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //rigidbodies.Remove(other.attachedRigidbody);
        if (other.attachedRigidbody.name.Contains("Car"))
        {
            GameplayScript script = other.attachedRigidbody.gameObject.GetComponent<GameplayScript>();

            if (rigidbodies.ContainsKey(script))
            {
                int temp = rigidbodies[script]--;

                temp--;

                if (temp == 0)
                {
                    rigidbodies.Remove(script);                    
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {        
        //Debug.Log(other.gameObject.name);

        if (other.attachedRigidbody.name.Contains("Car"))
        {
            GameplayScript gameplayScript = other.attachedRigidbody.gameObject.GetComponent<GameplayScript>();                        

            if(rigidbodies.ContainsKey(gameplayScript))
            {
                rigidbodies[gameplayScript]++;
            }

            if (gameplayScript.AddWaypointIndex(WaypointID))
            {
                Vector3 collisionPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);                

                gameplayScript.SetWaypointPosition(collisionPosition);
                gameplayScript.SetNormalToWaypoint(Normal);
                gameplayScript.IncreaseScore(BonusPointsForCrossing);
                
                //Debug.Log("Waypoint " + WaypointID + ", point: " + collisionPosition);                

                rigidbodies.Add(gameplayScript, 1);

                if(IsFinishLine)
                {
                    gameplayScript.ClearWaypointIndexes();
                }
            }
            else
            {                
                if(!rigidbodies.ContainsKey(gameplayScript))
                {
                    //Debug.Log("COFNĄŁ SIĘ - UBIT");
                    gameplayScript.EndGame();
                }
            }
        }
    }
}
