using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsToggles : MonoBehaviour
{
    [SerializeField]
    List<KeyToPanelBinding> Bindings;

    void Update()
    {
        foreach(KeyToPanelBinding bind in Bindings)
        {
            if(Input.GetKeyDown(bind.Key))
            {
                bind.Toggle();
            }
        }
    }
}

[System.Serializable]
public class KeyToPanelBinding
{
    public KeyCode Key;
    public GameObject Panel;
    private bool Shown = true;

    public void Toggle()
    {
        Shown = !Shown;
        Panel.SetActive(Shown);
    }
}
