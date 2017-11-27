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
        if(carManager != null && carManager.SimulationStarted)
        {
            Vector3 temp = carManager.AveragePositionAndRelax(); //carManager.AveragePosition();
            temp += new Vector3(0, 25 + 1.1f * carManager.MaxDistanceFromPoint(temp, 5), 0);

            if (!float.IsNaN(temp.x) && !float.IsNaN(temp.y) && !float.IsNaN(temp.z))
            {
                cameraPosition.position = temp;
            }
        }
    }
}
