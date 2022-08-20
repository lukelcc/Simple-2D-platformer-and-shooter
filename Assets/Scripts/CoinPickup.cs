using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    [SerializeField] AudioClip coinPickupSFX;
    [Range(0f, 1f)] [SerializeField] float volume = 1f;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<PlayerScore>().AddToScore(coinScore);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, volume);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
