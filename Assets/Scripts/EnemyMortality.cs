using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int hp = 3;

    [Header("Death and dismemberment")]
    [SerializeField] List<GameObject> bodyPartsList;
    [SerializeField] float yeetForce = 2f;
    [SerializeField] float rotatingSpeed = .2f;

    public int GetHealth()
    {
        return hp;
    }

   
    public void MinusHp(int hpToMinus)
    {
        hp = hp - hpToMinus;
        if (hp <= 0)
        {
            Die();
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
