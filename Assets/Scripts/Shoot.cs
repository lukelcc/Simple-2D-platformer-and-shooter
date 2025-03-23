using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] public int maxProjectilePoolSize=50;

    [SerializeField] public float projectileSpeed = 20f;//bullet speed
    [SerializeField] public int damage = 1;//damage per shot
    [SerializeField] public int magSize = 20;//bullets left in magazine
    [SerializeField] public float spread = 30f;//bullet spread
    [SerializeField] public int bulletsPerShot = 1;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;
    [SerializeField] public bool fullAutoMode = false;//semi/full-auto
    [SerializeField] public float firingRate = 1f;//rate of fire(cooldown between shots)
    public bool haveGun = false;//empty hand? cannot shoot
    private float shotgunBottomAngle;
    public bool isFiringPressed;
    private float[] shotgunArray;
    private bool onCooldown=false;

    Coroutine firingCoroutine;
    Coroutine firingShotgunCoroutine;

    //pool
    private IObjectPool<Projectile> projectilePool;

    //pool
    private void Awake()
    { 
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGet, OnRelease, onDestroyExtraProjectile, maxSize:maxProjectilePoolSize);
    }

    private void Start()
    {
        haveGun = false;
    }


    private void Update()
    {
        if (fullAutoMode)
            FireFullAuto();
    }

    private void FireFullAuto()//fire full-auto starts here
    {
        if (!haveGun||magSize <= 0)//check if got gun and ammo
        {
            Debug.Log("No gun/ammo!");
            return;
        }
        else if (Mouse.current.leftButton.isPressed && firingCoroutine == null && !onCooldown)
        {
            firingCoroutine = StartCoroutine(FireContinouosly());
        }
        else if (!Mouse.current.leftButton.isPressed && firingCoroutine != null || magSize <= 0)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;         
        }
    }

    IEnumerator FireContinouosly()//auto fire coroutine
    {
        while(true)
        {
            if (magSize <= 0)
                break;
            //projectilePool.Get();
            StartCoroutine(ShootBullet());
            magSize--;
            yield return new WaitForSeconds(firingRate);
        }
    }

    IEnumerator ShootShotgun()//shotgun
    {
        for (int shotgunBullet = 0; shotgunBullet < bulletsPerShot; shotgunBullet++)
        {

            firePoint.localRotation = Quaternion.Euler(new Vector3(firePoint.localRotation.x, firePoint.localRotation.y, shotgunArray[shotgunBullet]));
            projectilePool.Get();
            //shotgunBottomAngle += spread;
            yield return new WaitForSeconds(.02f);           
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
        //spread
        if (bulletsPerShot == 1)//for non shotgun
        {
            firePoint.localRotation = Quaternion.Euler(new Vector3(firePoint.localRotation.x, firePoint.localRotation.y, UnityEngine.Random.Range(-spread, spread)));
        }
        projectile.gameObject.SetActive(true);
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
             
        projectile.setProjectileDamage(damage);
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



    //private void ShootBullet()//check if shotgun or one bullet, then shoot bullet
    IEnumerator ShootBullet()
    {
        onCooldown = true;
        if (bulletsPerShot == 1)//non shotgun            
            projectilePool.Get();
        else//shotgun
        {
            shotgunArray = new float[bulletsPerShot];
            for (int i = 0; i < shotgunArray.Length; i++)
            {
                shotgunArray[i] = shotgunBottomAngle;
                shotgunBottomAngle += spread;               
            }          
            for (int posOfArray=0; posOfArray < bulletsPerShot; posOfArray++)
            {
                float tmp = shotgunArray[posOfArray];
                int randomArray= UnityEngine.Random.Range(0, posOfArray);
                shotgunArray[posOfArray] = shotgunArray[randomArray];
                shotgunArray[randomArray] = tmp;
            }

            shotgunBottomAngle = (-bulletsPerShot + 1) * spread/2;
            firingShotgunCoroutine = StartCoroutine(ShootShotgun());
        }
        yield return new WaitForSeconds(firingRate);
        onCooldown = false;
    }

    private void OnFire(InputValue value)//fire semi-auto
    {
        if (onCooldown)
            return;
        else if (!haveGun || magSize <= 0)//check if got gun and ammo
        {
            Debug.Log("No gun/ammo!");
            return;
        }
        else if (!fullAutoMode)
        {
            //projectilePool.Get();//shoot bullet
            StartCoroutine(ShootBullet());
            magSize--;//minus 1 ammo
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
