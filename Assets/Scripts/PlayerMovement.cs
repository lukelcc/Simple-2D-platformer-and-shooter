using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerRunSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbLadderSpeed = 5f;
    [SerializeField] float gravityScaleAtStart = 1f;

    private Vector2 moveInput;
 
    //colliders
    //boxCollider = feet
    //capsuleCollider = body


    // Update is called once per frame
    void Update()
    {
        //if (!isAlive) { return; } //make class dead or alive

        Run();
        FlipSprite();
        ClimbLadder();
        //Die(); //class dead or alive
    }

    //private void Die() //make class dead or alive - function
    //{
    //    if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
    //    {
    //        //isAlive = false;
    //        //GetComponent<Animator>().SetTrigger("Dying");
    //        //death fly away
    //        //GetComponent<Rigidbody2D>().velocity = deathKick;
    //        //Destroy(gameObject);
    //        //Instantiate(deathSprite, transform.position, Quaternion.identity);
    //        GetComponent<PlayerDeath>().Dismemberment();

    //    }
    //}


    //private void OnCollisionEnter2D(Collision2D collision) // when player get hurt by enemies/hazards
    //{
    //    if (collision.gameObject.tag == "CanHurtPlayer")
    //    {
    //        GetComponent<PlayerMortality>().Die();
    //        FindObjectOfType<GameSession>().MinusHp();
    //    }

    //}

    

    void OnMove(InputValue value) // getting WSAD key input from user
    {
        //if (!isAlive) { return; } //make class dead or alive

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) // getting space key from user
    {
        //if (!isAlive) { return; } //make class dead or alive

        //prevents double jumps
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        //set transformation
        Vector2 playerVelocity = new Vector2(moveInput.x*playerRunSpeed, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().velocity = playerVelocity;

        //change animation to running only if the player is moving left/right
        bool playerIsMovingHorinzontally = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        GetComponent<Animator>().SetBool("isRunning", playerIsMovingHorinzontally);
    }

    void FlipSprite()
    {
        bool playerIsMovingHorizontally = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon;
        if (playerIsMovingHorizontally)
        {
            //transform.localScale = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x), 1f); //left=-1, right=1
            if (Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) < 0) //left=-1, 
                GetComponent<SpriteRenderer>().flipX = true;
            else if (Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) > 0) //right = 1
                GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    private void ClimbLadder()
    {
        //only can climb when player touches ladder
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //if not climbing, gravity maintain the same
            GetComponent<Rigidbody2D>().gravityScale = gravityScaleAtStart;
            GetComponent<Animator>().SetBool("isClimbing", false);
            return;
        }
        //if climbing, set gravity to 0
        GetComponent<Rigidbody2D>().gravityScale = 0f;

        //set transformation for climbing    
        Vector2 climbLadderVelocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, moveInput.y * climbLadderSpeed);
        GetComponent<Rigidbody2D>().velocity = climbLadderVelocity;

        //change animation to climbing only if the player is moving up/down
        bool playerHasVerticalSpeedWhenClimbing = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > Mathf.Epsilon;
        GetComponent<Animator>().SetBool("isClimbing", playerHasVerticalSpeedWhenClimbing);

    }
}
