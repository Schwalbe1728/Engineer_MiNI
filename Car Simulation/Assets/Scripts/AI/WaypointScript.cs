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

    void OnTriggerEnter(Collider other)
    {        
        //Debug.Log(other.gameObject.name);

        if (other.attachedRigidbody.name.Contains("Car"))
        {
            GameplayScript gameplayScript = other.attachedRigidbody.gameObject.GetComponent<GameplayScript>();            

            if (gameplayScript.AddWaypointIndex(WaypointID))
            {
                Vector3 collisionPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                gameplayScript.SetWaypointPosition(collisionPosition);
                gameplayScript.IncreaseScore(BonusPointsForCrossing);
                //Debug.Log("Waypoint " + WaypointID + ", point: " + collisionPosition);

                if(IsFinishLine)
                {
                    gameplayScript.ClearWaypointIndexes();
                }
            }
        }
    }
}
