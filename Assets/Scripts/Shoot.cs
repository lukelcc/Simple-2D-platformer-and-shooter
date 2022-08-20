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
    [SerializeField] private int maxProjectilePoolSize=50;

    [SerializeField] private float projectileForce = 20f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    //pool
    private IObjectPool<Projectile> projectilePool;

    //pool
    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGet, OnRelease, onDestroyExtraProjectile, maxSize:maxProjectilePoolSize);
    }

   
    //pool
    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);
        projectile.SetPool(projectilePool);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
        return projectile;
    }

    //pool
    private void OnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    }
    //pool
    private void OnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    //pool
    private void onDestroyExtraProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    void OnFire(InputValue value)
    {
        projectilePool.Get();
        //fireprojectile();
    }
    //void FireProjectile()
    //{
    //    GameObject flyingProjectile = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);
    //    Rigidbody2D rb = flyingProjectile.GetComponent<Rigidbody2D>();
    //    rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    //    //GetComponent<Rigidbody2D>().AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    //}
}
