using NeuralNetwork.Core.Model.Neurons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronDefinitionsPanelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject NeuronDefinitionPanelPrefab;

    //private List<>

    public void AddNeuronDefinition()
    {
        GameObject added = Instantiate(NeuronDefinitionPanelPrefab, transform);

        added.GetComponentInChildren<Button>().
            onClick.
            AddListener(
                delegate { DeleteNeuronDefinition(added); }
            );

        added.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public void DeleteNeuronDefinition(GameObject toDestroy)
    {
        Debug.Log("Dafuq");
        Destroy(toDestroy);
    }

    public void SetNeuronDefinitions(int[] layerCount, List<System.Type> neuronType)
    {
        SetNeuronDefinitions(new List<int>(layerCount), neuronType);
    }

    public void SetNeuronDefinitions(List<int> layerCount, List<System.Type> neuronTypes)
    {
        Debug.Log("Layer Count: " + layerCount.Count);

        for(int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.name.Equals("Default Exit Neuron Panel"))
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < layerCount.Count - 1; i++)
        {
            AddNeuronDefinition();
        }

        for(int i = 0; i < layerCount.Count - 1; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            InputField inputField = child.GetComponentInChildren<InputField>();
            Dropdown dropdown = child.GetComponentInChildren<Dropdown>();

            inputField.text = layerCount[i].ToString();
            dropdown.value = GetNeuronTypeIndex(neuronTypes[i]);
            //ustaw typ
            //ustaw liczbę
        }
    }

    public void GetNeuronDefinitions(out List<int> layerCount, out List<System.Type> neuronTypes)
    {
        layerCount = new List<int>();
        neuronTypes = new List<System.Type>();

        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            InputField inputField = child.GetComponentInChildren<InputField>();
            Dropdown dropdown = child.GetComponentInChildren<Dropdown>();

            int layer;

            if(GetLayerCount(inputField.text, out layer))
            {
                layerCount.Add(layer);
                neuronTypes.Add(GetNeuronType(dropdown.captionText.text));
            }
            else
            {
                Debug.LogWarning("Nieprawidłowa liczba warstwy");
            }
        }
    }
	
    public System.Type GetNeuronType(string def)
    {
        switch(def.ToLower())
        {
            case "tanhneuron":
                return typeof(TanHNeuron);

            case "stepneuron":
                return typeof(StepNeuron);

            case "identityneuron":
                return typeof(IdentityNeuron);
        }

        throw new System.Exception("Błędny typ neuronu");
    }
    
    public int GetNeuronTypeIndex(System.Type type)
    {
        int result = -1;

        if(type == typeof(TanHNeuron))
        {
            result = 0;
        }

        if (type == typeof(StepNeuron))
        {
            result = 1;
        }

        if (type == typeof(IdentityNeuron))
        {
            result = 2;
        }

        return result;
    }

    public bool GetLayerCount(string def, out int layerCount)
    {
        return int.TryParse(def, out layerCount) && layerCount > 0;
    }

    public void ValidateLayerCountInput(string count)
    {
        int temp;

        if(GetLayerCount(count, out temp) && temp > 0)
        {

        }
    }
}
