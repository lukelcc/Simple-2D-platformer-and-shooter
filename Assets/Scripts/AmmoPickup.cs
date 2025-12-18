using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    bool wasCollected = false;

    public enum gameObjectTag
    {
        Player
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(gameObjectTag.Player.ToString()) && !wasCollected)
        {
            wasCollected = true;
            collision.gameObject.GetComponent<Shoot>().pickUpAmmo();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
