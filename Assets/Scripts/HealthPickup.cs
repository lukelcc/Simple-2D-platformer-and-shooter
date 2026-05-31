using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private AudioClip healSFX;
    [SerializeField][Range(0, 1)] private float healSFXVol = 0.7f;
    [SerializeField][Range(0, 30)] private int Hp = 10;
    public ItemSpawner itemSpawnerPrefab;

    bool wasCollected = false;

    public enum gameObjectTag
    {
        Player
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(gameObjectTag.Player.ToString()) && !wasCollected)
        {
            collision.gameObject.GetComponent<PlayerMortality>().AddHp(Hp);
            AudioSource.PlayClipAtPoint(healSFX, Camera.main.transform.position, healSFXVol);
            gameObject.SetActive(false);
            wasCollected = true;
            Instantiate(itemSpawnerPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
