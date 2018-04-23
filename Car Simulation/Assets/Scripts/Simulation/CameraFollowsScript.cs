using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsScript : MonoBehaviour
{
    [SerializeField]
    private CarsOnSceneManager carManager;    

    private Transform cameraPosition;
    private Vector3 velocity;
    private Camera camera;

    void Awake()
    {
        cameraPosition = this.transform;
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if(carManager != null && carManager.SimulationStarted)
        {
            Vector3 temp = carManager.AveragePositionAndRelax();
            temp += new Vector3(0, 25 + 1.1f * carManager.MaxDistanceFromPoint(temp, 5), 0);

            if (!float.IsNaN(temp.x) && !float.IsNaN(temp.y) && !float.IsNaN(temp.z))
            {
                cameraPosition.position =
                    Vector3.SmoothDamp(cameraPosition.position, temp, ref velocity, 0.4f);                
            }
        }
    }
}
