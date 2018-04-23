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

    public Vector3 AveragePositionAndRelax()
    {
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
    }

    public float MaxDistanceFromPoint(Vector3 point, float min = 30)
    {        
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
                SetIgnoreColliders(car1, car2);
            }

            CarsGameplayScripts[i] = CarsOnTheScene[i].gameObject.GetComponent<GameplayScript>();
        }

        CarColors = new Color[CarsOnTheScene.Length];
        SetCarColors(CarColors.Length);
    }

	void Awake ()
    {
        CarAdded();
	}
	
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
