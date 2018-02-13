using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarsOnSceneManager : MonoBehaviour
{
    public bool SimulationStarted { get { return simulationStarted; } }

    private bool simulationStarted = false;

    private Color[] CarColors;
    private Transform[] CarsOnTheScene;
    private Bounds CarRectBounds;
    private bool BoundsCreated = false;

    private GameplayScript[] CarsGameplayScripts;

    [SerializeField]
    private PopulationManagerScript populationManager;

    [SerializeField]
    private Text CarsLeftText;

    public void StartSimulation(float delay = 0f, bool checkForTimeout = true)
    {
        Debug.Log("CarsOnSceneManager.StartSimulation");

        if (delay <= 0f)
        {
            simulationStarted = true;
            foreach(GameplayScript gs in CarsGameplayScripts)
            {
                gs.Restart(checkForTimeout);
            }
        }
        else
        {
            StartCoroutine(DelayedStart(delay));
        }
    }    

    public void StopSimulation()
    {
        simulationStarted = false;
    }

    [System.Obsolete]
    public Vector3 AveragePosition()
    {
        Vector3 average = Vector3.zero;
        int activeCars = 0;

        if (CarsOnTheScene != null)
        {
            foreach (Transform car in CarsOnTheScene)
            {
                if (car.GetComponent<GameplayScript>().InProgress)
                {
                    average += car.position;
                    activeCars++;
                }
            }
        }
        else
            return Vector3.zero;        

        return (activeCars > 0)? average / activeCars : Vector3.zero;
    }

    public Vector3 AveragePositionAndRelax()
    {
        //CarRectBounds = new Bounds();

        bool foundFirst = false;

        foreach (GameplayScript car in CarsGameplayScripts)
        {
            if (car.InProgress)
            {
                if (!foundFirst)
                {
                    foundFirst = true;
                    CarRectBounds = new Bounds(car.gameObject.transform.position, Vector3.zero);
                }
                else
                {
                    CarRectBounds.Encapsulate(car.gameObject.transform.position);
                }
            }
        }        

        return CarRectBounds.center;

        #region Obsolete Previous Version
        /*
        Vector3 average = AveragePosition();

        if (CarsOnTheScene != null && CarsOnTheScene.Length > 1)
        {            
            Vector3 temp = Vector3.zero;
            float sumDist = 0;

            foreach (Transform car in CarsOnTheScene)
            {
                if (car.GetComponent<GameplayScript>().InProgress)
                {
                    float tm = Vector3.Distance(car.position, average);
                    sumDist += tm;

                    temp += car.position * tm;                    
                }
            }

            average = temp / (sumDist);
        }        

        return average;
        */
        #endregion
    }

    public float MaxDistanceFromPoint(Vector3 point, float min = 30)
    {
        /*
        float result = min;

        foreach(Transform car in CarsOnTheScene)
        {
            if(Vector3.Distance(point, car.position) > result && car.GetComponent<GameplayScript>().InProgress)
            {
                result = Vector3.Distance(point, car.position);
            }
        }

        return result;
        */
        float res = Mathf.Max(CarRectBounds.size.x, CarRectBounds.size.y, CarRectBounds.size.z);

        return (CarRectBounds == null || res < min) ? min : res;
    }

    public void CarAdded()
    {
        CarsOnTheScene = new Transform[transform.childCount];
        Debug.Log("Cars On Scene: " + transform.childCount.ToString());

        for (int i = 0; i < transform.childCount; i++)
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

	// Use this for initialization
	void Awake ()
    {
        CarAdded();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (simulationStarted)
        {
            int leftInGame = CarsOnTheScene.Length;

            foreach (GameplayScript script in CarsGameplayScripts)
            {
                if (!script.InProgress)
                {
                    leftInGame--;
                }
            }

            if (CarsLeftText != null) CarsLeftText.text = leftInGame.ToString();

            if (leftInGame == 0)
            {
                populationManager.RoundEnded();
                       
                foreach (GameplayScript script in CarsGameplayScripts)
                {
                    script.Restart(true);
                }

                //Debug.Log("Restart");
            }
        }
    }

    private void SetCarColors(int numberOfCars)
    {
        if (numberOfCars > 0)
        {
            float step = 1f / (numberOfCars - 1);
            float value = 0;

            for (int i = 0; i < numberOfCars; i++)
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
                    CarColors[i] = Color.Lerp(Color.green, Color.blue, (value - 0.5f) * 2);
                }

                CarsOnTheScene[i].GetChild(0).GetComponent<Renderer>().material.color = CarColors[i];

                value += step;
            }
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

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartSimulation();
    }
}
