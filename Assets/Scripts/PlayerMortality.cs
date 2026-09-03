using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    private TextMeshPro _label;


    //player label settings
    private Vector3 labelOffset = new Vector3(0f, 1f, 0f);//player label offset above sprite
    private float labelFontSize = 10f;//label font size
    private Color labelColor;



    private void Awake()
    {
        
    }

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
            BuildAndDisplayLabel(GetComponent<PlayerInput>().playerIndex+1, originaSpriteColor);
        }
        else if (GetComponent<PlayerInput>().playerIndex == 1)//P2
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(1).gameObject;           
            originaSpriteColor = Color.green;
            BuildAndDisplayLabel(GetComponent<PlayerInput>().playerIndex+1, originaSpriteColor);
        }
        else if (GetComponent<PlayerInput>().playerIndex == 2)//P3
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(2).gameObject;
            originaSpriteColor = Color.cyan;
            BuildAndDisplayLabel(GetComponent<PlayerInput>().playerIndex+1, originaSpriteColor);
        }
        else if (GetComponent<PlayerInput>().playerIndex == 3)//P4
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(3).gameObject;
            originaSpriteColor = Color.yellow;
            BuildAndDisplayLabel(GetComponent<PlayerInput>().playerIndex+1, originaSpriteColor);
        }
        playerUIObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = originaSpriteColor;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P1
        gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P2
        gameObject.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P3
        gameObject.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().color = originaSpriteColor;//P4
        GetComponent<PlayerAim>().SetCrosshairColor(originaSpriteColor);

        //HpBar.SetMaxHp(maxHp);
        //initialize hp bar to full hlp
        playerUIObject.GetComponentInChildren<HealthBar>().SetMaxHp(maxHp);
        playerUIObject.GetComponent<PlayerStats>().resetWeaponAndAmmo();

        //BuildLabel();

    }

    public enum gameObjectTag
    {
        Enemy,
        Hazard
    }

    //IEnumerator DestroyPlayerLabelCoroutine(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    _label.gameObject.SetActive(false);
    //}

    IEnumerator DisablePlayerLabel(float playerLabelDuration)
    {
        yield return new WaitForSeconds(playerLabelDuration);
        _label.gameObject.SetActive(false);
    }

    /// <summary>Show or hide (with fade) the P1/P2 label.</summary>
    //public void ShowLabel(bool show)
    //{
    //    if (_label == null) return;

    //    if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

    //    if (show)
    //    {
    //        _label.gameObject.SetActive(true);
    //        SetLabelAlpha(1f);
    //    }
    //    else
    //    {
    //        _fadeCoroutine = StartCoroutine(FadeLabel());
    //    }
    //}

    // ?? label construction ????????????????????????????????????????

    void BuildAndDisplayLabel(int playerNum, Color spriteColor)
    {
        // Create a world-space TextMeshPro object as a child
        var playerLabel = new GameObject($"Label_{playerNum}");
        playerLabel.transform.SetParent(transform, false);
        playerLabel.transform.localPosition = labelOffset;

        // Sorting: render above sprites (adjust layer/order as needed)
        playerLabel.layer = gameObject.layer;

        _label = playerLabel.AddComponent<TextMeshPro>();
        _label.text = "P"+playerNum.ToString();
        _label.fontSize = labelFontSize;
        _label.color = spriteColor;
        _label.alignment = TextAlignmentOptions.Center;
        _label.fontStyle = FontStyles.Bold;

        // Outline for readability over any background
        
        _label.outlineColor = Color.black;

        // Renderer order: draw on top of player sprite
        //GetComponent<MeshRenderer>().sortingLayerName = "Player";
        var r = playerLabel.GetComponent<MeshRenderer>();
        if (r != null) r.sortingLayerName = "Player";
        //Remove player label after the countdown
        StartCoroutine(DisablePlayerLabel(FindObjectOfType<GameSession>().playerLabelDuration));        
    }

    // ?? helpers ???????????????????????????????????????????????????

    //IEnumerator FadeLabel()
    //{
    //    float t = 0f;
    //    while (t < labelFadeDuration)
    //    {
    //        t += Time.deltaTime;
    //        SetLabelAlpha(1f - Mathf.Clamp01(t / labelFadeDuration));
    //        yield return null;
    //    }
    //    _label.gameObject.SetActive(false);
    //    SetLabelAlpha(1f); // reset for next time
    //}

    //void SetLabelAlpha(float a)
    //{
    //    if (_label == null) return;
    //    Color c = _label.color;
    //    c.a = a;
    //    _label.color = c;
    //    // Also fade outline
    //    Color oc = _label.outlineColor;
    //    oc.a = a;
    //    _label.outlineColor = oc;
    //}


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

    //heal
    public void AddHp(int HPtoAdd)
    {
        currentHp += HPtoAdd;
        if (currentHp >= maxHp)
            currentHp = maxHp;
        playerUIObject.GetComponentInChildren<HealthBar>().SetHp(currentHp);
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
        //playerUIObject.GetComponent<PlayerLife>().MinusLife();//edit
        //FindObjectOfType<GameSession>().PlayerDeath();
        //FindObjectOfType<PlayerStats>().resetWeaponAndAmmo();

        //reset the UI stats
        playerUIObject.GetComponent<PlayerStats>().resetWeaponAndAmmo();//edit
        playerUIObject.GetComponentInChildren<HealthBar>().SetMaxHp(maxHp);
        playerUIObject.SetActive(false);
        Dismemberment();
        //Debug.Log("Player: " + gameObject.name + " died");
    }

    private void Dismemberment()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
        FindObjectOfType<GameSession>().PlayerDeath();
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

}
