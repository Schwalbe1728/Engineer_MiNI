using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsOnSceneManager : MonoBehaviour
{
    private Color[] CarColors;
    private Transform[] CarsOnTheScene;

    private GameplayScript[] CarsGameplayScripts;

    public Vector3 AveragePosition()
    {
        Vector3 average = Vector3.zero;

        if (CarsOnTheScene != null)
        {                    
            foreach (Transform car in CarsOnTheScene)
            {
                average += car.position;
            }                        
        }

        return average / CarsOnTheScene.Length;
    }

    public Vector3 AveragePositionAndRelax()
    {
        Vector3 average = AveragePosition();

        if (CarsOnTheScene != null)
        {            
            Vector3 temp = Vector3.zero;
            float sumDist = 0;

            foreach (Transform car in CarsOnTheScene)
            {
                float tm = Vector3.Distance(car.position, average);
                sumDist += tm;

                temp += car.position * tm;
            }

            average = temp / (sumDist);
        }        

        return average;
    }

    public float MaxDistanceFromPoint(Vector3 point, float min = 30)
    {
        float result = min;

        foreach(Transform car in CarsOnTheScene)
        {
            if(Vector3.Distance(point, car.position) > result)
            {
                result = Vector3.Distance(point, car.position);
            }
        }

        return result;
    }

	// Use this for initialization
	void Awake ()
    {
        CarsOnTheScene = new Transform[transform.childCount];
        Debug.Log("Cars On Scene: " + transform.childCount.ToString());

        for(int i = 0; i < transform.childCount; i++)
        {
            CarsOnTheScene[i] = transform.GetChild(i);
        }

        CarsGameplayScripts = new GameplayScript[CarsOnTheScene.Length];

        for (int i = 0; i < CarsOnTheScene.Length; i++)
        {
            Collider[] car1 = CarsOnTheScene[i].gameObject.GetComponentsInChildren<Collider>();

            if (car1 == null) Debug.LogWarning("Nie znalazło collidera");

            for (int j = i + 1; j < CarsOnTheScene.Length; j++)
            {
                Collider[] car2 = CarsOnTheScene[j].gameObject.GetComponentsInChildren<Collider>();

                //Physics.IgnoreCollision(car1, car2);
                SetIgnoreColliders(car1, car2);
            }

            CarsGameplayScripts[i] = CarsOnTheScene[i].gameObject.GetComponent<GameplayScript>();
        }

        CarColors = new Color[CarsOnTheScene.Length];
        SetCarColors(CarColors.Length);
	}
	
	// Update is called once per frame
	void Update ()
    {
        int leftInGame = CarsOnTheScene.Length;

        foreach(GameplayScript script in CarsGameplayScripts)
        {
            if(!script.InProgress)
            {
                leftInGame--;
            }
        }

        if(leftInGame == 0)
        {
            foreach (GameplayScript script in CarsGameplayScripts)
            {
                script.Restart();
            }

            Debug.Log("Restart");
        }
    }

    private void SetCarColors(int numberOfCars)
    {
        float step = 1f / (numberOfCars - 1);
        float value = 0;

        for(int i = 0; i < numberOfCars; i++)
        {
            //R = 1 - value; //2 * Mathf.Max(0.5f - value, 0);
            //G = 1 - Mathf.Abs(0.5f - value) * 2;
            //B = 1 - R; //2 * Mathf.Max(value - 0.5f, 0);                        

            if (value < 0.5f)
            {
                CarColors[i] = Color.Lerp(Color.red, Color.green, value * 2);
            }
            else
            {
                CarColors[i] = Color.Lerp(Color.green, Color.blue, (value-0.5f) * 2);
            }

            CarsOnTheScene[i].GetChild(0).GetComponent<Renderer>().material.color = CarColors[i];

            value += step;
        }
    }

    private void SetIgnoreColliders(Collider[] car1, Collider[] car2)
    {
        for(int i = 0; i < car1.Length; i++)
        {
            for(int j = 0; j < car2.Length; j++)
            {
                Physics.IgnoreCollision(car1[i], car2[j]);
            }
        }
    }
}
