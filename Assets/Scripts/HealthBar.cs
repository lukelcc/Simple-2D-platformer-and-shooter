using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Image sliderFill;

    [Header("Color Settings")]
    public Gradient colorGradient;

    private void Start()
    {
        
    }

    public void SetMaxHp(int hp)
    {
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
        sliderFill.color = Color.green;
    }
    public void SetHp(int hp)
    {
        hpSlider.value = hp;
        //if (hpSlider.value > hpSlider.maxValue / 4 && hpSlider.value <= hpSlider.maxValue / 2)
        //{
        //    sliderFill.color = Color.yellow;
        //}
        //else if (hpSlider.value <= hpSlider.maxValue / 4)
        //{
        //    sliderFill.color = Color.red;
        //}
        float pct = Mathf.Clamp01(hpSlider.value / hpSlider.maxValue);
        sliderFill.color = colorGradient.Evaluate(pct);
    }


}
