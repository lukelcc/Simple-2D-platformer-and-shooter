using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;

    private void Start()
    {
        
    }

    public void SetMaxHp(int hp)
    {
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
    }
    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }


}
