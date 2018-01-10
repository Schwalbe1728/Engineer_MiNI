using NeuralNetwork.Core.Learning.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSimulationPanelScript : MonoBehaviour {

    [SerializeField]
    private GameObject CarsCollectionObject;

    [SerializeField]
    private GameObject InputSourcesCollectionObject;

    [SerializeField]
    private GameObject PopulationManagerObject;

    [SerializeField]
    private MultipleTrendPlotScript Plots;

    [SerializeField]
    private NoOfCarsPanel NumberOfCarsPanel;

    [SerializeField]
    private GameObject CarPrefab;

    [SerializeField]
    private GameObject HumanInputSourcePrefab;

    [SerializeField]
    private GameObject AIInputSourcePrefab;

    [SerializeField]
    private GameObject AISpecimanPrefab;

    [SerializeField]
    private Toggle HumanInputEnabled;

    [SerializeField]
    private InputField MutationChanceInputField;

    [SerializeField]
    private InputField SelectPercentInputField;

    [SerializeField]
    private Dropdown ParentChoosingDropdown;

    [SerializeField]
    private InputField SigmaInputField;

    public void StartSimulation()
    {
        Plots.RestartedSimulation();

        CarsOnSceneManager carsManager = CarsCollectionObject.GetComponent<CarsOnSceneManager>();

        for(int i = 0; i < NumberOfCarsPanel.NumberOfCars; i++)
        {
            GameObject car = Instantiate(CarPrefab, CarsCollectionObject.transform);

            car.GetComponent<GameplayScript>().SetStartPosition(CarsCollectionObject.transform);

            if ( i != 0 || !HumanInputEnabled.isOn )
            {
                GameObject AISpecimen = Instantiate(AISpecimanPrefab, PopulationManagerObject.transform);
                GameObject inputSource = Instantiate(AIInputSourcePrefab, InputSourcesCollectionObject.transform);
                AIInputSource AISource = inputSource.GetComponent<AIInputSource>();

                AISource.BindWithAI(AISpecimen.GetComponent<SpecimenScript>());

                car.GetComponent<OrdersReceiverScript>().SetInputSource(AISource);
                AISource.BindWithCar(car);
            }
            else
            {
                GameObject inputSource = Instantiate(HumanInputSourcePrefab, InputSourcesCollectionObject.transform);
                HumanInputSourceScript human = inputSource.GetComponent<HumanInputSourceScript>();

                car.GetComponent<OrdersReceiverScript>().SetInputSource(human);
            }
        }

        carsManager.CarAdded();        
        PopulationManagerScript popScript = PopulationManagerObject.GetComponent<PopulationManagerScript>();//.StartSimulation();        

        float mut, sel;

        if (!float.TryParse( MutationChanceInputField.text, out mut ))
        {
            Debug.LogWarning("Mutation Chance ma błędny format");
            string temp = MutationChanceInputField.text.Replace(".", ",");

            if (!float.TryParse(temp, out mut))
            {
                Debug.LogWarning("Mutation Chance ma błędny format... wait what");
                //MutationChanceInputField.text.Replace(".", ",");
            }
        }

        if (!float.TryParse(SelectPercentInputField.text, out sel))
        {
            Debug.LogWarning("Selection Chance ma błędny format");
            string temp = SelectPercentInputField.text.Replace(".", ",");

            if(!float.TryParse(temp, out sel))
            {

            }
        }

        if(!float.TryParse(SigmaInputField.text, out popScript.sigma))
        {
            string temp = SigmaInputField.text.Replace(".", ",");

            if(!float.TryParse(temp, out popScript.sigma))
            {

            }
        }

        popScript.SetParentChoosingMethod((ParentChoosingMethod)ParentChoosingDropdown.value);

        mut = Mathf.Clamp01(mut);
        sel = Mathf.Clamp01(sel);

        carsManager.StartSimulation(0f);
        popScript.StartSimulation(mut, sel);        

        HidePanel();       
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void StopSimulation()
    {
        CarsOnSceneManager carsManager = 
            CarsCollectionObject.GetComponent<CarsOnSceneManager>();

        carsManager.StopSimulation();
        carsManager.StopAllCoroutines();

         PopulationManagerScript populationScript =
            PopulationManagerObject.GetComponent<PopulationManagerScript>();

        populationScript.StopSimulation(true);
        populationScript.StopAllCoroutines();
    }

    public void DespawnSimulationObjects()
    {
        for(int i = 0; i < CarsCollectionObject.transform.childCount; i++)
        {
            Destroy(CarsCollectionObject.transform.GetChild(i).gameObject, 0.1f);
        }

        for (int i = 0; i < PopulationManagerObject.transform.childCount; i++)
        {
            Destroy(PopulationManagerObject.transform.GetChild(i).gameObject, 0.1f);
        }

        for (int i = 0; i < InputSourcesCollectionObject.transform.childCount; i++)
        {
            Destroy(InputSourcesCollectionObject.transform.GetChild(i).gameObject, 0.1f);
        }
    }
}
