using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Default movement settings")]
    [SerializeField] private float defaultRunSpeed = 6f;
    [SerializeField] private float defaultJumpSpeed = 12f;
    [SerializeField] private float defaultClimbLadderSpeed = 5f;
    private float defaultGravityScaleAtStart = 1f;
    [SerializeField] private float defaultBouncingForce = 20f;

    [Header("In-game Movement settings")]
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpSpeed = 12f;
    [SerializeField] private float climbLadderSpeed = 5f;
    private float gravityScaleAtStart = 1f;
    [SerializeField] private float bouncingForce = 20f;

    [Header("Drop down from bridge settings")]
    [SerializeField] private float dropDuration = 1f;
    [SerializeField] private float checkRadius = 1f;

    private Vector2 moveInput;

    Shoot shooter;

    //colliders
    //boxCollider = feet
    //capsuleCollider = body

    public void setPlayerDefaultMovementValues()
    {
        runSpeed = defaultRunSpeed;
        jumpSpeed = defaultJumpSpeed;
        climbLadderSpeed = defaultClimbLadderSpeed;
        //gravityScaleAtStart = defaultGravityScaleAtStart;
        bouncingForce = defaultBouncingForce;
    }

    public void changePlayerMovementFactor(float factor) //heavier weapon = slower movement
    {
        runSpeed=defaultRunSpeed*factor;
        jumpSpeed=defaultJumpSpeed*factor; 
        climbLadderSpeed=defaultClimbLadderSpeed*factor;
        bouncingForce=defaultBouncingForce*factor;
    }

    public enum gameObjectTag
    {
        Enemy,
        Hazard,
        Ladder,
        Bouncing,
        Bridge
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isAlive) { return; } //make class dead or alive

        Run();
        FlipSprite();
        ClimbLadder();
        //Bounce();
        //Die(); //class dead or alive
    }

    private void Start()
    {
        setPlayerDefaultMovementValues();
        StartCoroutine(TemporaryFreezePlayer(FindObjectOfType<GameSession>().countdownDuration));
    }

    IEnumerator TemporaryFreezePlayer(float duration)
    {
        GetComponent<PlayerInput>().DeactivateInput();
        yield return new WaitForSeconds(duration);
        GetComponent<PlayerInput>().ActivateInput();
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
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")) && !GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Bridge")))
        {
            return;
        } 

        //jump 
        if (value.isPressed)
        {
            GetComponent<Rigidbody2D>().velocity += new Vector2(0f, jumpSpeed);
        }
        
    }


    void Run()
    {
        //set transformation
        Vector2 playerVelocity = new Vector2(moveInput.x*runSpeed, GetComponent<Rigidbody2D>().velocity.y);
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
        if (!GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask(gameObjectTag.Ladder.ToString())))
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


    //drop down from bridge
    private Collider2D GetPlatformBelow()
    {
        // Check just below the player's feet for a one-way platform
        Vector2 checkPos = (Vector2)transform.position + Vector2.down * checkRadius;
        return Physics2D.OverlapCircle(checkPos, checkRadius, LayerMask.GetMask(gameObjectTag.Bridge.ToString()));
    }

    private IEnumerator DropThrough(Collider2D platform)
    {
        //Debug.Log("drop down");
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), platform, true);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), platform, true);
        yield return new WaitForSeconds(dropDuration);

        // Safety: only re-enable if the object still exists (player might've left the scene, etc.)
        if (platform != null)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), platform, false);
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), platform, false);
        }
    }

    void OnDropDown(InputValue value)
    {
        if(value.isPressed)
        {
            if(GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask(gameObjectTag.Bridge.ToString())))
            {
                Collider2D platform = GetPlatformBelow();
                if (platform != null)
                    StartCoroutine(DropThrough(platform));
            }
        }
    }


    private void Bounce()
    {
        //GetComponent<Rigidbody2D>().AddForce(bouncingForce*Vector2.up, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, bouncingForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask(gameObjectTag.Bouncing.ToString())))
        {
            //collision.gameObject.transform.localScale = new Vector3(2, 2);//???

            // This is your collision point in world space
            //Vector3 collisionPoint = collision.GetContact(0).point;
            //Debug.Log("bounce point:" + collisionPoint);
            //FindObjectOfType<BouncePadAnimation>().AnimateBouncePad(collision.GetContact(0).point);
            collision.gameObject.GetComponent<BouncePadAnimation>().AnimateBouncePad(collision.GetContact(0).point);
            //Debug.Log("touch point:"+collision.GetContact(0).point);
            Bounce();
        }
    }

    

}
