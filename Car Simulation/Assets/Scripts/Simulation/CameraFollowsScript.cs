using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsScript : MonoBehaviour
{
    [SerializeField]
    private CarsOnSceneManager carManager;    

    private Transform cameraPosition;

    void Awake()
    {
        cameraPosition = this.transform;
    }

    void Update()
    {
        if(carManager != null)
        {
            Vector3 temp = carManager.AveragePositionAndRelax(); //carManager.AveragePosition();
            temp += new Vector3(0, 25 + carManager.MaxDistanceFromPoint(temp, 5), 0);

            cameraPosition.position = temp;
        }
    }
}
