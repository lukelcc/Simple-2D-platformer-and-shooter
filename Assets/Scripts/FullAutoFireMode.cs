using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoFireMode : GunFireMode
{
    public override void UpdateUniqueGunStats()
    {
        base.UpdateCommonGunStats();
        playerObject.GetComponent<Shoot>().fullAutoMode = true;
        playerObject.GetComponent<Shoot>().bulletsPerShot = 1;
        playerObject.GetComponent<Shoot>().isBurst = false;
    }
}
