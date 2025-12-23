using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Transform aimOrigin;
    Vector3 mouseCursorPos;

    [SerializeField] GameObject ObjectsToRotate;

    //controller aim
    //[SerializeField] public GameObject crosshair;
    private Vector2 aimDirection;



    private void Awake()
    {
        //aimOrigin = transform.Find("HeadAndGun");
        aimOrigin = ObjectsToRotate.transform;
    }

    private void Start()
    {
        
    }

    public Vector2 getAimDirection()
    {
        return aimDirection;
    }


    void OnAim(InputValue value) // getting controller aim stick direction
    {
        if(value.Get<Vector2>() != Vector2.zero)//dont include stick move back
            aimDirection = value.Get<Vector2>();
    }

    private void Aim()
    {
        //mouse aiming
        //mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //edit       
        //Vector3 aimDirection = (mouseCursorPos - transform.position).normalized; //edit
        //aimDirection = (mouseCursorPos - transform.position).normalized; //edit

        if (aimDirection == Vector2.zero)
            return;
      
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimOrigin.eulerAngles = new Vector3(0, 0, angle);

        //player turn around when looking angle < 90 or > -90'
        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = 1f;
        }
        aimOrigin.localScale = aimLocalScale;
    }

    //aim using controller
    //private void Aim()
    //{
    //    crosshair.transform.localPosition=movement
    //}


    private void Update()
    {
        Aim();
    }
}
