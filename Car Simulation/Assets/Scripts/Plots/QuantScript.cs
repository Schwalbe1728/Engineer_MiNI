using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantScript : MonoBehaviour {

    [SerializeField]
    private Text QuantValueText;

    public void SetText(float val, string format)
    {
        QuantValueText.text = val.ToString(format);
    }
}
