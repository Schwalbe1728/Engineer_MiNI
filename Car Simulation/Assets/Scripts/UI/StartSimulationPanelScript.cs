﻿using System.Collections;
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

    public void StartSimulation()
    {
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
}
