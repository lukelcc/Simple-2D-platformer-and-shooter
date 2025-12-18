using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public event Action onWeaponPickup;
    public event Action onAmmoChange;

    private int ammoLeft=0;
    private string modelName="";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getModelName()
    {
        return modelName;
    }

    public int getAmmoLeft()
    {
        return ammoLeft;
    }

    public void resetWeaponAndAmmo()
    {
        modelName = "";
        ammoLeft = 0;
        if (onWeaponPickup != null)
        {
            onWeaponPickup();
        }
        if (onAmmoChange != null)
        {
            onAmmoChange();
        }
    }

    public void pickupWeapon(string modelName)
    {
        this.modelName = modelName;
        if(onWeaponPickup != null)
        {
            onWeaponPickup();
        }
    }

    public void newAmmoMag(int ammoLeft)
    {
        this.ammoLeft = ammoLeft;
        if (onAmmoChange != null)
        {
            onAmmoChange();
        }
    }

    public void minusAmmoByOne()
    {
        ammoLeft--;
        if (onAmmoChange != null)
        {
            onAmmoChange();
        }
    }
}
