using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.InputSystem.PlayerInputManager;

public class PlayerMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int startingLife = 3;
    [SerializeField] private int maxHp = 30;
    private int currentHp;
    public HealthBar HpBar;

    [Header("Injured")]
    [SerializeField] private float injuredSpriteColorPeriod = .1f;
    [SerializeField] private float controlDisablePeriod = 3f;
    [SerializeField] private float knockBackForce = 20f;

    [Header("Death and dismemberment")]
    [SerializeField] List<GameObject> bodyPartsList;
    [SerializeField] float yeetForce = 20f;
    [SerializeField] float rotatingSpeed = 2f;

    GameObject playerUIObject;
    Color originaSpriteColor;
    //PlayerInputManager playerInputManager;

    int lastFrame = 0;
    private void Start()
    {
        currentHp = maxHp;
        //FindObjectOfType<HealthBar>().SetMaxHp(maxHp);


        //to link to UI
        if (GetComponent<PlayerInput>().playerIndex == 0)//P1
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(0).gameObject;
            originaSpriteColor = Color.white;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 1)//P2
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(1).gameObject;           
            originaSpriteColor = Color.green;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 2)//P3
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(2).gameObject;
            originaSpriteColor = Color.blue;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 3)//P4
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(3).gameObject;
            originaSpriteColor = Color.black;
        }
        playerUIObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = originaSpriteColor;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P1
        gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P2
        gameObject.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P3
        gameObject.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P4
        //HpBar.SetMaxHp(maxHp);
        //initialize hp bar to full hlp
        playerUIObject.GetComponentInChildren<HealthBar>().SetMaxHp(maxHp);
        playerUIObject.GetComponent<PlayerStats>().resetWeaponAndAmmo();



    }

    public enum gameObjectTag
    {
        Enemy,
        Hazard
    }


    public int GetStartingLife()
    {
        return startingLife;
    }


    public void MinusHp(int damage)
    {
        currentHp -= damage;
        StartCoroutine(toggleInjuredSprite());
        //HpBar.SetHp(currentHp);
        playerUIObject.GetComponentInChildren<HealthBar>().SetHp(currentHp);
        if (currentHp <= 0)
            if (Time.frameCount > lastFrame)
            {
                lastFrame = Time.frameCount;
                Die();
            }
                
    }
    //private void OnCollisionEnter2D(Collision2D collision) // when player get hurt by enemies/hazards
    //{
    //    if (collision.gameObject.tag.Equals(gameObjectTag.Enemy.ToString()) || collision.gameObject.tag.Equals(gameObjectTag.Hazard.ToString()))
    //    {
    //        // Get the direction from the impact point to this object
    //        Vector2 direction = (transform.position - collision.transform.position).normalized;           
    //        direction.Normalize();

    //        StartCoroutine(knockBackPlayer(direction));

    //        MinusHp(1);
    //        //Die();
            
    //        //FindObjectOfType<GameSession>().MinusLife();
    //        //FindObjectOfType<PlayerStats>().resetWeaponAndAmmo();
    //    }
    //}

    private IEnumerator toggleInjuredSprite()//color when got injured/shot
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(injuredSpriteColorPeriod);
        GetComponent<SpriteRenderer>().color = originaSpriteColor;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = originaSpriteColor;
        gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = originaSpriteColor;

    }

    //private IEnumerator knockBackPlayer(Vector2 direction)
    //{
    //    GetComponent<SpriteRenderer>().color = Color.red;
    //    GameObject.Find("Head").GetComponent<SpriteRenderer>().color = Color.red;
    //    GameObject.Find("Grip").GetComponent<SpriteRenderer>().color = Color.red;
    //    GetComponent<PlayerMovement>().enabled = false;
    //    GetComponent<Rigidbody2D>().velocity = new Vector2(knockBackForce, knockBackForce);
    //    // Apply the knockback force
    //    GetComponent<Rigidbody2D>().AddForce(direction * knockBackForce, ForceMode2D.Impulse);

    //    yield return new WaitForSeconds(controlDisablePeriod);
    //    GetComponent<PlayerMovement>().enabled = true;
    //    GetComponent<SpriteRenderer>().color = Color.white;
    //    GameObject.Find("Head").GetComponent<SpriteRenderer>().color = Color.white;
    //    GameObject.Find("Grip").GetComponent<SpriteRenderer>().color = Color.white;
    //}

    //private IEnumerator TemporaryDisablePlayerMovement()
    //{
    //    //GetComponent<PlayerMovement>().enabled = false;
    //    GetComponent<PlayerInput>().DeactivateInput();

    //    GetComponent<SpriteRenderer>().color = Color.red;
    //    yield return new WaitForSeconds(controlDisablePeriod);

    //    //GetComponent<PlayerMovement>().enabled = true;
    //    GetComponent<PlayerInput>().ActivateInput();

    //    GetComponent<SpriteRenderer>().color = Color.white;
    //}

    public void Die()
    {
        //FindObjectOfType<PlayerLife>().MinusLife();
        //playerInputManager.DisableJoining();
        playerUIObject.SetActive(false);
        playerUIObject.GetComponent<PlayerLife>().MinusLife();
        //FindObjectOfType<PlayerStats>().resetWeaponAndAmmo();

        //reset the UI stats
        playerUIObject.GetComponent<PlayerStats>().resetWeaponAndAmmo();//edit
        playerUIObject.GetComponentInChildren<HealthBar>().SetMaxHp(maxHp);
        playerUIObject.SetActive(false);
        //StartCoroutine(TemporaryDisablePlayerMovementAndVisibility());
        Dismemberment();
        Debug.Log("Player: " + gameObject.name + " died");
    }

    private void Dismemberment()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);

        Transform firePoint = GetComponent<Transform>().transform;


        for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
        {
            //Debug.Log("parts:" + bodyPartsIndex);
            GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
            flyingBodyParts.GetComponent<SpriteRenderer>().color = originaSpriteColor;
            Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

            Vector2 yeetDirection = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
            rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
            rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
        }
    }

    private IEnumerator TemporaryDisablePlayerMovementAndVisibility()//Player die, temporary disabled, reset health//edit
    {

        //disable player controls
        //GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().DeactivateInput();

        //loss all ammo and gun
        //GetComponent<Shoot>().ammoLeft = 0;
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;//disable gun sprite renderer


        //disable sprite renderer
        gameObject.GetComponent<SpriteRenderer>().enabled=false;      
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;//head
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().enabled = false;//grip

        //disable collider
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //disable rigid body
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;

        //destroy gun
        //Destroy(transform.GetChild(0).GetChild(1).GetChild(0).gameObject);

        //GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(controlDisablePeriod);

        //GetComponent<PlayerMovement>().enabled = true;
        //enable sprite renderer
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;//head
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().enabled = true;//grip

        //enable collider
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        //enable rigid body
        gameObject.GetComponent<Rigidbody2D>().simulated = true;

        //enable player controls
        GetComponent<PlayerInput>().ActivateInput();

        playerUIObject.SetActive(true);

        //loss all ammo and gun
        //GetComponent<Shoot>().ammoLeft = 0;
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;//enable gun sprite renderer

        currentHp = maxHp;
    }

}
