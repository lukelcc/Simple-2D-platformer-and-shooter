using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item          // ← NOT a MonoBehaviour
{
    public GameObject itemPrefab;
    [Range(0, 10)] public int spawnWeight;
}

public class ItemSpawner : MonoBehaviour
{
    [Range(0, 30)] public int firstSpawnCooldown = 10;//first spawn delay in seconds
    [Range(0, 30)] public int subsequentSpawnCooldown = 20;//subsequent spawn delay in seconds
    private int spawnCooldown;

    public Item[] item_array;//the whole array of item
    Item SelectItemByPorbability() //constructor to choose item based on probability
    {
        int totalWeight = 0;
        foreach (var item in item_array)
            if (item != null) totalWeight += item.spawnWeight;

        if (totalWeight <= 0f)
        {
            Debug.LogError("All item weights are 0. Nothing can spawn.");
            return null;
        }

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var item in item_array)
        {
            if (item == null) continue;
            cumulative += item.spawnWeight;
            if (roll <= cumulative) return item;
        }
        return null;
    }

    private void Start()
    {
        if (FindObjectOfType<GameSession>().HasTheFirstItemAlreadySpawned(firstSpawnCooldown) == false)//the first items have not spawn
        {
            spawnCooldown = firstSpawnCooldown;
            //Debug.Log(spawnCooldown);
        }

        else//the first items have spawn
        {
            spawnCooldown = subsequentSpawnCooldown;
            //Debug.Log(spawnCooldown);
        }

        Item selectedItem = SelectItemByPorbability();
        StartCoroutine(SpawnItem(selectedItem));
    }

    IEnumerator SpawnItem(Item item)
    {
        yield return new WaitForSeconds(spawnCooldown);
        Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);//destroy spawner after spawning item
    }
}
