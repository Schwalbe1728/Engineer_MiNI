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
    }

    public void DeleteNeuronDefinition(GameObject toDestroy)
    {
        Debug.Log("Dafuq");
        Destroy(toDestroy);
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

    public bool GetLayerCount(string def, out int layerCount)
    {
        return int.TryParse(def, out layerCount) && layerCount > 0;
    }
}
