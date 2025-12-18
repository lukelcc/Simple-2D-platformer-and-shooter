using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiShotgunFireMode : GunFireMode
{
    [SerializeField] private int bulletsPerRound = 7;

    public override void UpdateUniqueGunStats()
    {
        base.UpdateCommonGunStats();
        playerObject.GetComponent<Shoot>().bulletsPerShot = bulletsPerRound;
        playerObject.GetComponent<Shoot>().fullAutoMode = false;
        playerObject.GetComponent<Shoot>().isBurst = false;
    }
}
