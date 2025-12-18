using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBurstFireMode : GunFireMode
{
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float burstFiringRate = .07f;

    public override void UpdateUniqueGunStats()
    {
        base.UpdateCommonGunStats();
        playerObject.GetComponent<Shoot>().bulletsPerShot = bulletsPerBurst;
        playerObject.GetComponent<Shoot>().burstFiringRate = burstFiringRate;
        playerObject.GetComponent<Shoot>().fullAutoMode = true;
        playerObject.GetComponent<Shoot>().isBurst = true;
    }
}
