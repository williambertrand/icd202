using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: this could easily be geeneeralizablee to any slider script if we add other
// resources (hunger, thirst, etc)
public class EnergyBar : MonoBehaviour
{

    public Slider energySlider;


    public void SetMaxValue(float value)
    {
        energySlider.maxValue = value;
        energySlider.value = value;
    }

    public void SetValue(float value)
    {
        energySlider.value = value;
    }
}
