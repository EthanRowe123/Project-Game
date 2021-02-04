using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetGravityValues(float minGrav, float maxGrav)
    {
        slider.minValue = minGrav;
        slider.maxValue = maxGrav;
        slider.value = 1;
        fill.color = gradient.Evaluate(1f);
    }
    public void setGravity(float gravity)
    {
        slider.value = gravity;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
