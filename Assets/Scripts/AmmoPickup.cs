using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    bool wasCollected = false;
    public ItemSpawner itemSpawnerPrefab;
    public enum gameObjectTag
    {
        Player
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(gameObjectTag.Player.ToString()) && !wasCollected)
        {
            try
            {
                collision.gameObject.GetComponent<Shoot>().pickUpAmmo();
                gameObject.SetActive(false);
                wasCollected = true;
                Instantiate(itemSpawnerPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }catch(UnassignedReferenceException)
            {
                Debug.Log("Empty hand, cannot pick up ammo");
            }
        }
    }
}
