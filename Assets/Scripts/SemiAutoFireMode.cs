using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoFireMode : GunFireMode
{
    public override void UpdateUniqueGunStats()
    {
        base.UpdateCommonGunStats();
        playerObject.GetComponent<Shoot>().bulletsPerShot = 1;
        playerObject.GetComponent<Shoot>().fullAutoMode = false;
        playerObject.GetComponent<Shoot>().isBurst = false;
    }
}
