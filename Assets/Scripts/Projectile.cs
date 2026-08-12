using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private float projectileSpeed = 30;
    [SerializeField] private int numBounce = 0;

    //[SerializeField] private GameObject impactExplosion;
    [SerializeField] private ParticleSystem bulletImpactFx;

    //pool
    private IObjectPool<Projectile> projectilePool;
    //pool
    public void SetPool(IObjectPool<Projectile> pool)
    {
        projectilePool = pool;
    }

    //pool
    //private void OnBecameInvisible()
    //{
    //    projectilePool.Release(this);
    //}
    public enum gameObjectTag
    {
        Player
    }

    public int getProjectileDamage()
    {
        return projectileDamage;
    }
    public void setProjectileDamage(int newDamage)
    {
        this.projectileDamage = newDamage;
    }

    public void setProjectileSpeed(float newSpeed)
    {
        this.projectileSpeed = newSpeed;
    }

    public void setNumBounce(int numBounce)
    {
        if (numBounce > 0)
            GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 1;
        else
            GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 0;//bullet cannot bounce
        this.numBounce = numBounce;
    }


    public void playBulletImpactFx()
    {
        if (bulletImpactFx != null)
        {
            ParticleSystem instance = Instantiate(bulletImpactFx, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax); // destroy after the particles fx anim
        }
    }


    private void destroyBullet()
    {
        //GetComponent<Rigidbody2D>().velocity = Vector2.zero;      
        projectilePool.Release(this);
        playBulletImpactFx();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {               
        if (collision.gameObject.GetComponent<Projectile>())//the bullet cannot hit another bullet
            return;

        if (collision.gameObject.layer == 6 && numBounce > 0)//bullet bounce
        {
            --numBounce;
            return;
        }
      
        //if (collision.gameObject.tag == "Enemy") //if bullet hit enemy goomba
        //{
        //    try
        //    {
        //        collision.gameObject.GetComponent<EnemyMortality>().MinusHp(projectileDamage); // do damage to enemy
        //    }
        //    catch (MissingReferenceException error)
        //    {
        //        Debug.Log(error.Message);
        //    }
        //}
        if (collision.gameObject.tag.Equals(gameObjectTag.Player.ToString()))//if bullet hit other player
        {
            collision.gameObject.GetComponent<PlayerMortality>().MinusHp(projectileDamage);
            //destroyBullet();
        }

        destroyBullet();
        //projectilePool.Release(this);
        //playBulletImpactFx();
        //Destroy(gameObject);       
        //Instantiate(impactExplosion, transform.position, Quaternion.identity); //instantiate bullet impact
    }

}
