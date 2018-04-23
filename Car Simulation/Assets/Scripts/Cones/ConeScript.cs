using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ConeScript : MonoBehaviour
{        	
    void OnCollisionEnter(Collision collision)
    {
        GameplayScript gs = collision.gameObject.GetComponent<GameplayScript>();

        if(gs != null)
        {
            gs.EndGame();
        }
        else
        {
            Debug.Log("NIEWŁAŚCIWY OBIEKT WYKRYWA KOLIZJĘ");
        }
    }
}
