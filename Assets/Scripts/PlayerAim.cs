using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Transform aimOrigin;
    Vector3 mouseCursorPos;

    [SerializeField] GameObject ObjectsToRotate;

    private void Awake()
    {
        //aimOrigin = transform.Find("HeadAndGun");
        aimOrigin = ObjectsToRotate.transform;
    }

    private void Aim()
    {
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 aimDirection = (mouseCursorPos - transform.position).normalized;
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

    private void Update()
    {
        Aim();
    }
}
