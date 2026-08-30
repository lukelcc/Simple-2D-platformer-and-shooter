using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    //link to player stats
    //[SerializeField] public GameObject playerStatsObj;

    //When pickup weapon, instanstiate a weapon spawner object
    public WeaponSpawner weaponSpawnerPrefab;
    //projectile pool settings

    [SerializeField] public Projectile ProjectilePrefab;
    [SerializeField] public int maxProjectilePoolSize = 50;
    //common gun stats
    public string modelName;
    public Transform firePoint;
    public float firingRate = 1f;//rate of fire(cooldown between shots)
    public float projectileSpeed = 20f;//bullet speed
    //public bool bigBullet = false;
    public int damage = 1;//damage per shot
    public int ammoLeft = 0;//bullets left in magazine
    public int magSize = 0;//magazine capacity
    public float spread = 30f;//bullet spread
    //variation gun stats
    public int bulletsPerShot = 1;
    public bool fullAutoMode = false;//semi/full-auto  
    public bool isBurst = false;
    public float burstFiringRate = .05f;//burst firing rate

    public int numBounce = 0;//bullet bounce on wall
    [SerializeField] public AudioClip gunShotSFX;
    [SerializeField][Range(0, 1)] public float gunShotSFXVol = 0.7f;
    [SerializeField] public AudioClip gunCockSFX;
    [SerializeField][Range(0, 1)] public float gunCockSFXVol = 0.7f;

    //muzzle flash settings
    [SerializeField] public GameObject muzzleFlash;
    [SerializeField] public int NumFramesToFlash = 10;


    public bool haveGun = false;//empty hand? cannot shoot
    //private float shotgunBottomAngle;
    private float[] shotgunArray;
    [SerializeField] private bool onCooldown=false;
    //if !onCooldown, can fire
    //if onCooldown, cannot fire

    public bool withinPickupRange = false;//check if item in pickup range
    public GameObject weaponWithinPickupRange;//check if weapon within pickup range
    [SerializeField] private float pickeupFireDelay = 1f;

    private bool muzzleIsFlashing = false;

    Coroutine firingCoroutine;

    //pool
    private IObjectPool<Projectile> projectilePool;

    //so that only one controller is read
    private Gamepad currentGamepad;
    private Mouse currentMouse;

    GameObject playerUIObject;

    //unity tags
    public enum gameObjectTag
    {
        Weapon,
        Ammo
    }

    public enum gameObjectName
    {
        P1,
        P2,
        P3,
        P4
    }


    //pool
    private void Awake()
    { 
        //projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGet, OnRelease, onDestroyExtraProjectile, maxSize:maxProjectilePoolSize);
    }

    public void CreateProjectilePool()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGet, OnRelease, onDestroyExtraProjectile, maxSize: maxProjectilePoolSize);
    }

    private void Start()
    {
        haveGun = false;
        magSize = 0;
        ammoLeft = 0;
        muzzleFlash.SetActive(false);

        //to link to UI
        if (GetComponent<PlayerInput>().playerIndex == 0)
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(0).gameObject;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 1)
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(1).gameObject;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 2)
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(2).gameObject;
        }
        else if (GetComponent<PlayerInput>().playerIndex == 3)
        {
            playerUIObject = FindObjectOfType<GameSession>().gameObject.transform.GetChild(0).GetChild(3).gameObject;
        }

        //to avoid multiple controller conflict 
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.devices.Count > 0)
        {
            foreach (var device in playerInput.devices)
            {
                if (device is Gamepad gamepad)
                {
                    currentGamepad = gamepad;
                    break;
                }
                if (device is Mouse mouse)//edit
                {
                    currentMouse = mouse;
                    //Debug.Log("Mouse");
                    break;
                }//end edit
            }
        }

    }


    private void Update()
    {
        if (fullAutoMode)
            FireFullAuto();       
    }

    private void FireFullAuto()//fire full-auto starts here
    {
        if (!haveGun||ammoLeft <= 0)//check if got gun and ammo
        {
            //Debug.Log("No gun/ammo!");
            return;
        }

        //mouse input
        else if (currentMouse != null)//edit    
        {
            if (Mouse.current.leftButton.isPressed && firingCoroutine == null && !onCooldown)
            {
                firingCoroutine = StartCoroutine(FireContinouosly());
            }
            else if (!Mouse.current.leftButton.isPressed && firingCoroutine != null || ammoLeft <= 0)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = null;
            }
        }
        //else if (currentGamepad.rightTrigger.isPressed && firingCoroutine == null && !onCooldown)
        else if (( currentGamepad.rightTrigger.isPressed|| currentGamepad.xButton.isPressed) && firingCoroutine == null && !onCooldown)
        //else if (( currentGamepad.leftTrigger.isPressed|| currentGamepad.rightTrigger.isPressed) && firingCoroutine == null && !onCooldown)
        //else if (Mouse.current.leftButton.isPressed && firingCoroutine == null && !onCooldown)
        {
            firingCoroutine = StartCoroutine(FireContinouosly());           
        }
        //else if (!currentGamepad.rightTrigger.isPressed && firingCoroutine != null || ammoLeft <= 0)
        else if (( !currentGamepad.rightTrigger.isPressed|| !currentGamepad.xButton.isPressed) && firingCoroutine != null || ammoLeft <= 0)
        //else if (( !currentGamepad.leftTrigger.isPressed|| !currentGamepad.rightTrigger.isPressed) && firingCoroutine != null || ammoLeft <= 0)
        //else if (!Mouse.current.leftButton.isPressed && firingCoroutine != null || ammoLeft <= 0)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;         
        }
    }

    IEnumerator FireContinouosly()//auto fire coroutine
    {
        while(true)
        {
            if (ammoLeft <= 0)
                break;
            //projectilePool.Get();
            
            StartCoroutine(ShootBullet());
            ammoLeft--;
            //update ammo UI
            //playerStatsObj.GetComponent<PlayerStats>().minusAmmoByOne();
            playerUIObject.GetComponent<PlayerStats>().minusAmmoByOne();
            yield return new WaitForSeconds(firingRate);
        }
    }

    //IEnumerator ShootShotgun()//shotgun
    ////private void ShootShotgun()
    //{
    //    AudioSource.PlayClipAtPoint(gunShotSFX, Camera.main.transform.position, gunShotSFXVol);
    //    for (int shotgunBullet = 0; shotgunBullet < bulletsPerShot; shotgunBullet++)
    //    {
    //        firePoint.localRotation = Quaternion.Euler(new Vector3(firePoint.localRotation.x, firePoint.localRotation.y, shotgunArray[shotgunBullet]));
    //        projectilePool.Get();
    //        //shotgunBottomAngle += spread;
    //        yield return new WaitForSeconds(.02f);           
    //    }
    //}

    //IEnumerator ShootShotgun()
    private void ShootShotgun()//shotgun
    {
        AudioSource.PlayClipAtPoint(gunShotSFX, Camera.main.transform.position, gunShotSFXVol);
        float shotgunBottomAngle = (-bulletsPerShot + 1) * spread / 2;     
        for (int shotgunBullet = 0; shotgunBullet < bulletsPerShot; shotgunBullet++)
        {
            firePoint.localRotation = Quaternion.Euler(new Vector3(firePoint.localRotation.x, firePoint.localRotation.y, shotgunBottomAngle));
            projectilePool.Get();
            shotgunBottomAngle += spread;
            //yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator ShootBurst()//shoot burst
    {
        for(int burstBullet=0;burstBullet<bulletsPerShot;burstBullet++)
        {
            projectilePool.Get();
            yield return new WaitForSeconds(burstFiringRate);//delay between burst
        }
    }

    //pool
    private Projectile CreateProjectile()//instantiate bullet
    {       
        Projectile projectile = Instantiate(ProjectilePrefab, firePoint.position,firePoint.rotation);  
        projectile.SetPool(projectilePool);
        return projectile;
    }

    //pool
    private void OnGet(Projectile projectile)//shoot bullet
    {
        //toggle muzzle flash
        if (!muzzleIsFlashing)
            StartCoroutine(ToggleMuzzleFlash());
        //spread
        if (bulletsPerShot == 1 || isBurst)//for single bullet and burst
        {
            AudioSource.PlayClipAtPoint(gunShotSFX, Camera.main.transform.position, gunShotSFXVol);
            firePoint.localRotation = Quaternion.Euler(new Vector3(firePoint.localRotation.x, firePoint.localRotation.y, UnityEngine.Random.Range(-spread, spread)));
        }
        projectile.gameObject.SetActive(true);
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
             
        projectile.setProjectileDamage(damage);
        projectile.setProjectileSpeed(projectileSpeed);
        projectile.setNumBounce(numBounce);
        //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //rb.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);
        projectile.GetComponent<Rigidbody2D>().velocity = firePoint.right * projectileSpeed;


    }
    //pool
    private void OnRelease(Projectile projectile)//recycle bullet
    {
        projectile.gameObject.SetActive(false);
    }
    //pool
    private void onDestroyExtraProjectile(Projectile projectile)//destroy extra bullet
    {
        Destroy(projectile.gameObject);
    }

    public void changeMuzzleFlashPosition()
    {
        muzzleFlash.transform.position = firePoint.position;
    }
    //toggle muzzle flash
    IEnumerator ToggleMuzzleFlash()
    {      
        muzzleFlash.SetActive(true);
        var NumFramesFlashed = 0;
        muzzleIsFlashing = true;

        while (NumFramesFlashed <= NumFramesToFlash)
        {
            NumFramesFlashed++;
            yield return null;            
        }
        muzzleFlash.SetActive(false);
        muzzleIsFlashing = false;
    }

    //private void ShootBullet()//check shooting mode(single bullet/burst/shotgun), then shoot bullet
    IEnumerator ShootBullet()
    {    
        onCooldown = true;
        if (bulletsPerShot == 1)//non shotgun/burst            
            projectilePool.Get();
        else//shotgun/burst
        {
            if (isBurst)//burst
            {
                StartCoroutine(ShootBurst());
            }
            else//shotgun
            {
                //shotgunArray = new float[bulletsPerShot];
                //for (int i = 0; i < shotgunArray.Length; i++)
                //{
                //    shotgunArray[i] = shotgunBottomAngle;
                //    shotgunBottomAngle += spread;
                //}
                //for (int posOfArray = 0; posOfArray < bulletsPerShot; posOfArray++)
                //{
                //    float tmp = shotgunArray[posOfArray];
                //    int randomArray = UnityEngine.Random.Range(0, posOfArray);
                //    shotgunArray[posOfArray] = shotgunArray[randomArray];
                //    shotgunArray[randomArray] = tmp;
                //}
                //shotgunBottomAngle = (-bulletsPerShot + 1) * spread / 2;
                //StartCoroutine(ShootShotgun());
                ShootShotgun();
            }
        }
        yield return new WaitForSeconds(firingRate);
        onCooldown = false;
    }

    private void OnFire(InputValue value)//fire semi-auto starts here
    {
        if (onCooldown)
            return;
        else if (!haveGun || ammoLeft <= 0)//check if got gun and ammo
        {
            Debug.Log("No gun/ammo!");
            return;
        }
        else if (!fullAutoMode)
        {
            //projectilePool.Get();//shoot bullet
            StartCoroutine(ShootBullet());
            ammoLeft--;//minus 1 ammo
            //update ammo UI
            playerUIObject.GetComponent<PlayerStats>().minusAmmoByOne();
            //playerStatsObj.GetComponent<PlayerStats>().minusAmmoByOne();
        }
    }

    void OnPickUp(InputValue value)//Press F to pickup/pickup button on controller
    {
        //if (withinPickupRange && !Mouse.current.leftButton.isPressed && !onCooldown) //cannot pickup when existing weapon is firing
        if (withinPickupRange && !onCooldown) //cannot pickup when existing weapon is firing //edit
        {
            //shoot delay after pickup
            StartCoroutine(PickupFireCoolDown());
            //instantiate a weapon spawner object
            Instantiate(weaponSpawnerPrefab, weaponWithinPickupRange.transform.position, Quaternion.identity);
            weaponWithinPickupRange.GetComponent<GunFireMode>().pickUpWeapon(GetComponent<Transform>());//update weapon stats
            AudioSource.PlayClipAtPoint(gunCockSFX, Camera.main.transform.position, gunCockSFXVol);//play gun cock audio
            ammoLeft = magSize;//refill magazine
            playerUIObject.GetComponent<PlayerStats>().newAmmoMag(ammoLeft);//update ammo UI
            playerUIObject.GetComponent<PlayerStats>().pickupWeapon(modelName);//update gun UI
            //playerStatsObj.GetComponent<PlayerStats>().newAmmoMag(ammoLeft);//update ammo UI
            //playerStatsObj.GetComponent<PlayerStats>().pickupWeapon(modelName);//update gun UI
            
            
        }
    }

    IEnumerator PickupFireCoolDown()//cannot fire for a while when pickup weapon
    {       
        onCooldown = true;
        yield return new WaitForSeconds(pickeupFireDelay);
        onCooldown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision) //items within range to pickup
    {
        if (collision.gameObject.tag.Equals(gameObjectTag.Weapon.ToString()))//weapons
        {
            withinPickupRange = true;
            weaponWithinPickupRange = collision.gameObject;
            weaponWithinPickupRange.GetComponent<GunFireMode>().highlightWeapon(true);
        }
        //else if (collision.gameObject.tag.Equals(gameObjectTag.Ammo.ToString()) && haveGun)//ammo, cannot pick up if no gun
        //{
        //    Destroy(collision.gameObject);
        //    AudioSource.PlayClipAtPoint(gunCockSFX, Camera.main.transform.position, gunCockSFXVol);
        //    ammoLeft += magSize;//refill magazine
        //    //FindObjectOfType<PlayerStats>().newAmmoMag(ammoLeft);
        //}
    }

    public void pickUpAmmo()
    {
        AudioSource.PlayClipAtPoint(gunCockSFX, Camera.main.transform.position, gunCockSFXVol);
        ammoLeft += magSize;//refill magazine
        playerUIObject.GetComponent<PlayerStats>().newAmmoMag(ammoLeft);
        
        //playerStatsObj.GetComponent<PlayerStats>().newAmmoMag(ammoLeft);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag.Equals(gameObjectTag.Ammo.ToString()) && haveGun)//ammo, cannot pick up if no gun
    //    {
    //        Destroy(collision.gameObject);
    //        AudioSource.PlayClipAtPoint(gunCockSFX, Camera.main.transform.position, gunCockSFXVol);
    //        ammoLeft += magSize;//refill magazine
    //        //FindObjectOfType<PlayerStats>().newAmmoMag(ammoLeft);            
    //    }

    //}

    private void OnTriggerExit2D(Collider2D collision) //weapon outside range of pickup
    {
        if (collision.gameObject.tag.Equals(gameObjectTag.Weapon.ToString()))
        {          
            withinPickupRange = false;
            weaponWithinPickupRange = collision.gameObject;
            weaponWithinPickupRange.GetComponent<GunFireMode>().highlightWeapon(false);
            weaponWithinPickupRange = null;          
        }       
    }
    //void FireProjectile()
    //{
    //    GameObject flyingProjectile = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);
    //    Rigidbody2D rb = flyingProjectile.GetComponent<Rigidbody2D>();
    //    rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    //    //GetComponent<Rigidbody2D>().AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    //}
}
