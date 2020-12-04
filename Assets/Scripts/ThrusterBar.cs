using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{

    //Thruster Slider on UI
    public Slider _thrusterBarSlider;
    public Gradient _thrusterBarGradient;
    public Image _thrusterFill;

    public void SetMaxThruster(float _thrusterLevel)
    {
        //Slider levels
        _thrusterBarSlider.maxValue = _thrusterLevel;
        _thrusterBarSlider.value = _thrusterLevel;
        
        //Gradient effect of Yellow to Red on Thruster Bar
        _thrusterFill.color = _thrusterBarGradient.Evaluate(1f);
    }

    public void SetThrusterLevel(float _thrusterLevel)
    {
        _thrusterBarSlider.value = _thrusterLevel;

        //Synching Thruster Fill Level to Gradient
        _thrusterFill.color = _thrusterBarGradient.Evaluate(_thrusterBarSlider.normalizedValue);
    }

}
