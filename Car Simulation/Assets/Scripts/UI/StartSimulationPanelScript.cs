using NeuralNetwork.Core.Learning;
using NeuralNetwork.Core.Learning.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
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

    private LearningProcess process;

    public void LoadLearningProcess()
    {
        PopulationManagerScript popScript = PopulationManagerObject.GetComponent<PopulationManagerScript>();
        process = popScript.LoadLearningProcess( GetOpenFilePath() );

        if(process != null)
        {
            ParentChoosingDropdown.value = (int)process.LearningAlgorithm.Config.ParentMethod;
            ParentChoosingDropdown.RefreshShownValue();

            MutationChanceInputField.text = process.LearningAlgorithm.Config.MutationChance.ToString();
            SelectPercentInputField.text = process.LearningAlgorithm.Config.PercentToSelect.ToString();

            SigmaInputField.text = process.LearningAlgorithm.Config.RandOptions.Sigma.ToString();

            while(NumberOfCarsPanel.NumberOfCars > process.PopulationCount)
            {
                NumberOfCarsPanel.LessCars();
            }

            while (NumberOfCarsPanel.NumberOfCars < process.PopulationCount)
            {
                NumberOfCarsPanel.MoreCars();
            }
        }
    }

    public void ForceSaveInConfigurationMenu(bool overridePrevious)
    {
        float mut, sel;

        PopulationManagerScript popScript = PassValuesToPopulationManager(out mut, out sel);
        popScript.MakeLearningProcess(NumberOfCarsPanel.NumberOfCars, mut, sel, overridePrevious);

        if(overridePrevious)
        {
            process = null;
        }

        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            switch (sfd.ShowDialog())
            {
                case DialogResult.OK:
                    Debug.Log(sfd.FileName);

                    popScript.SaveLearningProcess(sfd.FileName);

                    break;
            }
        }
    }
    
    public void StartShowcase()
    {
        CarsOnSceneManager carsManager = CarsCollectionObject.GetComponent<CarsOnSceneManager>();
        GameObject car = Instantiate(CarPrefab, CarsCollectionObject.transform);
        car.GetComponent<GameplayScript>().SetStartPosition(CarsCollectionObject.transform);
        GameObject AISpecimen = Instantiate(AISpecimanPrefab, PopulationManagerObject.transform);
        GameObject inputSource = Instantiate(AIInputSourcePrefab, InputSourcesCollectionObject.transform);
        AIInputSource AISource = inputSource.GetComponent<AIInputSource>();

        AISource.BindWithAI(AISpecimen.GetComponent<SpecimenScript>());

        car.GetComponent<OrdersReceiverScript>().SetInputSource(AISource);
        AISource.BindWithCar(car);

        carsManager.CarAdded();
        carsManager.StartSimulation();
        HidePanel();
    }
    
    public void StartSimulation()
    {
        Plots.RestartedSimulation(process);

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

        float mut, sel;        

        carsManager.StartSimulation(0f);
        PassValuesToPopulationManager(out mut, out sel).StartSimulation(mut, sel, true);        

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

    private PopulationManagerScript PassValuesToPopulationManager(out float mut, out float sel)
    {
        PopulationManagerScript popScript = PopulationManagerObject.GetComponent<PopulationManagerScript>();

        if (!float.TryParse(MutationChanceInputField.text, out mut))
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

            if (!float.TryParse(temp, out sel))
            {
                Debug.LogWarning("Selection Chance ma błędny format... wait what");
            }

        }

        if (!float.TryParse(SigmaInputField.text, out popScript.sigma))
        {
            string temp = SigmaInputField.text.Replace(".", ",");

            if (!float.TryParse(temp, out popScript.sigma))
            {

            }
        }

        popScript.SetParentChoosingMethod((ParentChoosingMethod)ParentChoosingDropdown.value);

        mut = Mathf.Clamp01(mut);
        sel = Mathf.Clamp01(sel);

        return popScript;
    }

    private string GetOpenFilePath()
    {
        string result = null;

        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Multiselect = false;
            ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    Debug.Log(ofd.FileName);
                    result = ofd.FileName;
                    break;
            }
        }

        return result;
    }
}
