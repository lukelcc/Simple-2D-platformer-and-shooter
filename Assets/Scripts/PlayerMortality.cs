using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int strartingHp = 3;

    [Header("Death and dismemberment")]
    [SerializeField] List<GameObject> bodyPartsList;
    [SerializeField] float yeetForce = 20f;
    [SerializeField] float rotatingSpeed = 2f;

    private int hp;

    public int GetStartingHealth()
    {
        return strartingHp;
    }

    private void OnCollisionEnter2D(Collision2D collision) // when player get hurt by enemies/hazards
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Hazard")
        {
            Die();
            FindObjectOfType<GameSession>().MinusHp();
        }
    }

    public void Die()
    {
        Dismemberment();
    }

    private void Dismemberment()
    {
        Destroy(gameObject);

        Transform firePoint = GetComponent<Transform>().transform;


        for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
        {
            GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
            Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

            Vector2 yeetDirection = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
            rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
            rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
        }
    }
}
