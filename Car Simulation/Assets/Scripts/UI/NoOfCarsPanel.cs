using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoOfCarsPanel : MonoBehaviour
{
    [SerializeField]
    private int MinCars;

    [SerializeField]
    private int MaxCars;

    [SerializeField]
    private Text NumberOfCarsText;

    private int numberOfCars;


    public int NumberOfCars { get { return numberOfCars; } }

    public void LessCars()
    {
        if(NumberOfCars > MinCars)
        {
            numberOfCars--;
            UpdateDisplayedNumber();
        }
    }

    public void MoreCars()
    {
        if (NumberOfCars < MaxCars)
        {
            numberOfCars++;
            UpdateDisplayedNumber();
        }
    }

    void Awake()
    {
        numberOfCars = 70;
        UpdateDisplayedNumber();
    }

    private void UpdateDisplayedNumber()
    {
        NumberOfCarsText.text = NumberOfCars.ToString();
    }
}
