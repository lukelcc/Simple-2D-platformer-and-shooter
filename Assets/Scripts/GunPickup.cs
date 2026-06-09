using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunPickup : MonoBehaviour
{
    [SerializeField] public string modelName;//gun name
    [SerializeField] public int magSize = 20;//bullets in magazine
    [SerializeField] public bool isFullAuto = false;//semi/full-auto
    [SerializeField] public bool isBurst = false;
    [SerializeField] public float firingRate = 0;//rate of fire, cooldown between shots
    [SerializeField] public float spread = 10f;//bullet spread  
    [SerializeField] public float range = 10f;
    [SerializeField] public int damage = 1;//damage per bullet
    [SerializeField] public float projectileSpeed = 20f;//speed of bullet
    [SerializeField] public int bulletsPerShot = 1;//for shotgun
    [SerializeField] public float recoilForce = 1f;
    [SerializeField] public float reloadTime = 1f;
    [SerializeField] AudioClip shootSound;
    //[SerializeField][Range(0, 1)] float shootSoundVolume = 0.25f;

    private Transform grip;
    Vector3 mouseCursorPos;
    //private SpriteRenderer existingWeapon;
    // Start is called before the first frame update
    void Start()
    {
        grip=GameObject.Find("Grip").GetComponent<Transform>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool checkIsFacingBack()
    {
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 aimDirection = (mouseCursorPos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (angle > 90 || angle < -90)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Destroy(grip.GetChild(0).gameObject);//replace existing weapon
            transform.SetParent(grip);
            if (checkIsFacingBack())//check if facing back to flip the gun
            {
                GetComponent<SpriteRenderer>().flipY = true;
            }
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.identity;
            UpdateGunStats();
        }
    }

    private void UpdateGunStats()//upadate gun stats
    {
        FindObjectOfType<Shoot>().haveGun = true;
        FindObjectOfType<Shoot>().firingRate = firingRate;
        FindObjectOfType<Shoot>().projectileSpeed = projectileSpeed;
        FindObjectOfType<Shoot>().fullAutoMode = isFullAuto;
        FindObjectOfType<Shoot>().damage = damage;
        FindObjectOfType<Shoot>().ammoLeft = magSize;
        FindObjectOfType<Shoot>().spread = spread;
        FindObjectOfType<Shoot>().bulletsPerShot = bulletsPerShot;
        FindObjectOfType<Shoot>().isBurst = isBurst;
        Debug.Log("Picked up: "+modelName);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
