using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponListByTier          // ← NOT a MonoBehaviour
{
    [Range(1, 5)] public int tier;
    public GunFireMode[] weaponPrefabList;
    [Range(0, 20)] public int spawnWeight;
}

//[System.Serializable]
public class WeaponSpawner : MonoBehaviour
{
    [Range(0, 30)] public int spawnCooldown = 10;//spawn delay in seconds

    public WeaponListByTier[] wlbt;//the whole array of weapon tiers
    WeaponListByTier SelectWeaponTierByPorbability() //constructor to choose weapon tier based on probability
    {
        int totalWeight = 0;
        foreach (var weaponTier in wlbt)
            if (weaponTier != null) totalWeight += weaponTier.spawnWeight;

        if (totalWeight <= 0f)
        {
            Debug.LogError("All weapon weights are 0. Nothing can spawn.");
            return null;
        }

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var weaponTier in wlbt)
        {
            if (weaponTier == null) continue;
            cumulative += weaponTier.spawnWeight;
            if (roll <= cumulative) return weaponTier;
        }

        return null;
    }

    //[Range(1, 5)] public int tier; // 1 = common, 5 = legendary

    //[SerializeField] private int spawnInterval = 3;
    //public GameObject[] weaponPrefabList;





    private void Start()
    {
        WeaponListByTier selectedTier = SelectWeaponTierByPorbability();//select only one weapon tier.
        //Debug.Log("selected tier:" + selectedTier.tier);
        StartCoroutine(SelectAndSpawnWeaponFromTier(selectedTier));
    }

    

    IEnumerator SelectAndSpawnWeaponFromTier(WeaponListByTier SelectedWeaponTier)//choose a weapon from the selected tier
    {
        yield return new WaitForSeconds(spawnCooldown);
        //random roll to select a weapon
        int roll = Random.Range(0,SelectedWeaponTier.weaponPrefabList.Length);
        //Debug.Log("Tier:" + SelectedWeaponTier.tier + ", num guns:" + SelectedWeaponTier.weaponPrefabList.Length + ", gun choice:" + roll);
        //spawn weapon
        Instantiate(SelectedWeaponTier.weaponPrefabList[roll], transform.position, Quaternion.identity);
        Destroy(gameObject);//destroy spawner after spawning weapon
    }


    //WeaponEntry selected = PickWeapon();
    //IEnumerator SpawnWeapon()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(spawnInterval);
    //        SpawnOneWeapon();
    //    }

    //}
    //private void Start()
    //{
    //    StartCoroutine(SpawnOneWeapon());
    //}



    //IEnumerator SpawnOneWeapon()
    //{
    //    yield return new WaitForSeconds(spawnInterval);
    //    int randomIndex = UnityEngine.Random.Range(0, weaponPrefabList.Length);
    //    GameObject weapon = Instantiate(weaponPrefabList[randomIndex], transform.position, Quaternion.identity) as GameObject;
    //    //GetComponent<BoxCollider2D>().offset = weapon.GetComponent<BoxCollider2D>().offset;
    //    //GetComponent<BoxCollider2D>().size = weapon.GetComponent<BoxCollider2D>().size;
    //    Destroy(gameObject);
    //}
}
