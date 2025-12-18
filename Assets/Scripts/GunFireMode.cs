using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GunFireMode : MonoBehaviour
{
    [SerializeField] public string modelName;//gun name

    private Transform firePoint;
    private Transform gripLocation;

    private enum WeaponClass
    {
        Pistol,
        SMG,
        Assault_rifle,
        Rifle,
        Shotgun,
        LMG,
        Sniper_rifle
    }

    [SerializeField] WeaponClass weaponClass;
    [SerializeField][Range(1, 10)] private int spawnChance=5;
    [SerializeField] private int magSize = 30;//bullets in magazine
    [SerializeField] private float firingRate = .5f;//rate of fire, cooldown between shots
    [SerializeField] private float spread = 2;//bullet spread  
    [SerializeField] private int damage = 3;//damage per bullet
    [SerializeField] private Projectile ProjectilePrefab;
    [SerializeField] private float projectileSpeed = 30;//speed of bullet
    //[SerializeField] public float reloadTime = 1;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private SpriteRenderer pickupSprite;
    
    [SerializeField] private AudioClip gunShotSFX; 
    [SerializeField][Range(0, 1)] private float gunShotSFXVol = 1f;
    [SerializeField] private AudioClip gunCockSFX;
    [SerializeField][Range(0, 1)] private float gunCockSFXVol = 1f;

    private Transform grip;
    protected GameObject playerObject;
    Vector3 mouseCursorPos;

    public const int _PISTOL = 10, 
        _SMG = 8, 
        _AR = 6, 
        _RIFLE = 4,
        _SHOTGUN = 6, 
        _LMG = 2,
        _SR = 1;

    // Start is called before the first frame update
    void Start()
    {
        //grip = GameObject.Find("Grip").GetComponent<Transform>();       
        firePoint = transform.GetChild(0).gameObject.transform;//set bullet spawn point
    }

    public void highlightWeapon(bool isPickup)
    {
        if (isPickup)
            GetComponent<SpriteRenderer>().sprite = pickupSprite.sprite;
        else
            GetComponent<SpriteRenderer>().sprite = normalSprite.sprite;
    }

    
    public bool checkIsFacingBack()
    {
        //mouse aiming
        //mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //controller aiming

        //Vector2 aimDirection = GameObject.Find("Player").GetComponent<PlayerAim>().getAimDirection();
        Vector2 aimDirection = playerObject.GetComponent<PlayerAim>().getAimDirection();

        //mouse aiming
        //Vector3 aimDirection = (mouseCursorPos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
            return true;
        else
            return false;       
    }
    //public void OnTriggerEnter2D(Collider2D collision) //Pickup weapon when player bump into weapon
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(grip.GetChild(0).gameObject);//replace existing weapon
    //        transform.SetParent(grip);
    //        if (checkIsFacingBack())//check if facing back to flip the gun
    //        {
    //            GetComponent<SpriteRenderer>().flipY = true;
    //        }
    //        transform.localPosition = Vector2.zero;
    //        transform.localRotation = Quaternion.identity;
    //        UpdateUniqueGunStats();
    //    }
    //}

    public void pickUpWeapon(Transform playerTransform)//pick up weapon when press F
    {

        //Destroy(grip.GetChild(0).gameObject);//replace existing weapon
        playerObject = playerTransform.gameObject;//assign the player object
        Debug.Log(playerObject);
        grip = playerTransform.GetChild(0).GetChild(1);//assign the player grip    
        Debug.Log(grip);
        Debug.Log(grip.GetChild(0));
        Destroy(grip.GetChild(0).gameObject);
        
        transform.SetParent(grip);
        if (checkIsFacingBack())//check if facing back to flip the gun
        {
            GetComponent<SpriteRenderer>().flipY = true;
            firePoint.localPosition = new Vector3(firePoint.localPosition.x, -firePoint.localPosition.y, firePoint.localPosition.z);
            //readjust the grip to the gun trigger
            grip.transform.localPosition = new Vector3(grip.transform.localPosition.x, +firePoint.transform.localPosition.y, grip.transform.localPosition.z);
        }else
            grip.transform.localPosition = new Vector3(grip.transform.localPosition.x, -firePoint.transform.localPosition.y, grip.transform.localPosition.z);
        
        transform.localPosition = Vector2.zero;
        transform.localRotation = Quaternion.identity;      
        

        UpdateUniqueGunStats();
    }

    public void UpdateCommonGunStats()
    {
        //FindObjectOfType<Shoot>().modelName = modelName;
        //FindObjectOfType<Shoot>().firePoint = firePoint;       
        //FindObjectOfType<Shoot>().firingRate = firingRate;
        //FindObjectOfType<Shoot>().projectileSpeed = projectileSpeed;
        //FindObjectOfType<Shoot>().damage = damage;
        //FindObjectOfType<Shoot>().magSize = magSize;
        //FindObjectOfType<Shoot>().spread = spread;
        //Debug.Log("Picked up: " + modelName);
        //FindObjectOfType<Shoot>().haveGun = true;
        //GetComponent<BoxCollider2D>().enabled = false;
        //FindObjectOfType<Shoot>().changeMuzzleFlashPosition();
        //FindObjectOfType<Shoot>().gunShotSFX = gunShotSFX;
        //FindObjectOfType<Shoot>().gunShotSFXVol = gunShotSFXVol;
        //FindObjectOfType<Shoot>().gunCockSFX = gunCockSFX;
        //FindObjectOfType<Shoot>().gunCockSFXVol = gunCockSFXVol;
        //FindObjectOfType<Shoot>().ProjectilePrefab = ProjectilePrefab;
        //FindObjectOfType<Shoot>().CreateProjectilePool();


        playerObject.GetComponent<Shoot>().modelName = modelName;
        playerObject.GetComponent<Shoot>().firePoint = firePoint;
        playerObject.GetComponent<Shoot>().firingRate = firingRate;
        playerObject.GetComponent<Shoot>().projectileSpeed = projectileSpeed;
        playerObject.GetComponent<Shoot>().damage = damage;
        playerObject.GetComponent<Shoot>().magSize = magSize;
        playerObject.GetComponent<Shoot>().spread = spread;
        Debug.Log("Picked up: " + modelName);
        playerObject.GetComponent<Shoot>().haveGun = true;
        GetComponent<BoxCollider2D>().enabled = false;
        playerObject.GetComponent<Shoot>().changeMuzzleFlashPosition();
        playerObject.GetComponent<Shoot>().gunShotSFX = gunShotSFX;
        playerObject.GetComponent<Shoot>().gunShotSFXVol = gunShotSFXVol;
        playerObject.GetComponent<Shoot>().gunCockSFX = gunCockSFX;
        playerObject.GetComponent<Shoot>().gunCockSFXVol = gunCockSFXVol;
        playerObject.GetComponent<Shoot>().ProjectilePrefab = ProjectilePrefab;
        playerObject.GetComponent<Shoot>().CreateProjectilePool();

    }
    public abstract void UpdateUniqueGunStats();//upadate gun stats
    
}
