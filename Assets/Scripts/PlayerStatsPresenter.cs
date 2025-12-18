using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsPresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoLeftText;
    [SerializeField] TextMeshProUGUI modelName;

    // Start is called before the first frame update
    void Start()
    {
        //initialize the value to null when start.
        ammoLeftText.text = "0";
        modelName.text = "";
        GetComponent<PlayerStats>().onAmmoChange += UpdateAmmoCount;
        GetComponent<PlayerStats>().onWeaponPickup += UpdateModelName;
        UpdateAmmoCount();
        UpdateModelName();
    }


    void UpdateAmmoCount()
    {
        ammoLeftText.text = GetComponent<PlayerStats>().getAmmoLeft().ToString();
    }

    void UpdateModelName()
    {
        modelName.text = GetComponent<PlayerStats>().getModelName().ToString();
    }
}
