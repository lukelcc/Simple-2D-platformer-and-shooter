using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMortality : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int startingLife = 3;
    [SerializeField] private int maxHp = 30;
    private int currentHp;
    public HealthBar HpBar;

    [Header("Injured")]
    [SerializeField] private float injuredSpriteColorPeriod = .1f;
    [SerializeField] private float controlDisablePeriod = 1f;
    [SerializeField] private float knockBackForce = 20f;

    [Header("Death and dismemberment")]
    [SerializeField] List<GameObject> bodyPartsList;
    [SerializeField] float yeetForce = 20f;
    [SerializeField] float rotatingSpeed = 2f;

    GameObject playerUIObject;
    Color originaSpriteColor;

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
            originaSpriteColor = Color.cyan;
        }
        playerUIObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = originaSpriteColor;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P1
        gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P2
        gameObject.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P3
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
            Die();
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
        
        playerUIObject.SetActive(false);playerUIObject.GetComponent<PlayerLife>().MinusLife();
        //FindObjectOfType<PlayerStats>().resetWeaponAndAmmo();
        //reset the UI stats
        playerUIObject.GetComponent<PlayerStats>().resetWeaponAndAmmo();
        playerUIObject.GetComponentInChildren<HealthBar>().SetMaxHp(maxHp);
        playerUIObject.SetActive(false);
        Dismemberment();
    }

    private void Dismemberment()
    {
        Destroy(gameObject);

        Transform firePoint = GetComponent<Transform>().transform;


        for (int bodyPartsIndex = 0; bodyPartsIndex < bodyPartsList.Count; bodyPartsIndex++)
        {
            GameObject flyingBodyParts = Instantiate(bodyPartsList[bodyPartsIndex], firePoint.position, firePoint.rotation);
            flyingBodyParts.GetComponent<SpriteRenderer>().color = originaSpriteColor;
            Rigidbody2D rb = flyingBodyParts.GetComponent<Rigidbody2D>();

            Vector2 yeetDirection = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
            rb.AddForce(yeetDirection * yeetForce, ForceMode2D.Impulse);
            rb.AddTorque(rotatingSpeed, ForceMode2D.Impulse);
        }
    }
}
