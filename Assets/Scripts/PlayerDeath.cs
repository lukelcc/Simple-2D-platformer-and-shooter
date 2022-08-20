using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDeath : MonoBehaviour
{
    [SerializeField] List<GameObject> bodyPartsList;

    [SerializeField] float yeetForce = 20f;
    [SerializeField] float rotatingSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die() // needs new class
    {
        //FindObjectOfType<GameSession>().ProcessPlayerDeath();
        Dismemberment();
    }

    private void Dismemberment() //needs new class
    {
        Destroy(gameObject);

        Transform firePoint = GetComponent<Transform>().transform;
        

        for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
        {        
            GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
            Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

            Vector2 yeetDirection = new Vector2(Random.Range(-5, 5), Random.Range(5,10));
            rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
            rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
        }
        
        // Instantiate(deathSprite, transform.position, Quaternion.identity);
    }
        
    
}
