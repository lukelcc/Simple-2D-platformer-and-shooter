using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private int spawnInterval = 10;
    [SerializeField] private int maxWeaponSpawn = 5;
    public GameObject[] weaponPrefabList;

    //level 1 - pistols, smg
    //level 2 - ar, shotgun
    //level 3 - rifle, lmg, sr


    public void SpawnWeapon()
    {
        
    }
}
