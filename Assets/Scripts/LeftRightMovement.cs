using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0f);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemySprite();
    }

    private void FlipEnemySprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x)), 1f);

    }
}
