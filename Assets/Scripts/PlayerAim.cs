using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerAim : MonoBehaviour
{
    private Transform aimOrigin;
    //private Transform crosshair;
    Vector3 mouseCursorPos;

    [SerializeField] GameObject ObjectsToRotate;
    [SerializeField] GameObject Crosshair;
    [SerializeField] private float CrosshairDistance = 5f;
    //[SerializeField] private float CrosshairMinDistance = 3f;
    //[SerializeField] bool inverseCrosshairDistance = true;

    //crosshair within camera 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float edgePadding = 1f;
    //private bool isAimStickUsed = false;

    //controller aim
    private Vector2 aimDirection;

    private Gamepad currentGamepad;
    private Mouse currentMouse;


    private void Awake()
    {
        //aimOrigin = transform.Find("HeadAndGun");
        aimOrigin = ObjectsToRotate.transform;
        
    }

    public void changeCrosshairDistance(float newCrosshairDistance)
    {
        CrosshairDistance = newCrosshairDistance;
    }

    private Vector3 ClampToCameraView(Vector3 worldPos)
    {
        float camHeight = mainCamera.orthographicSize;
        //float camWidth = camHeight * mainCamera.aspect;
        float camWidth = camHeight * (float)(16.0/9.0);

        Vector3 camPos = mainCamera.transform.position;

        worldPos.x = Mathf.Clamp(worldPos.x, camPos.x - camWidth + edgePadding, camPos.x + camWidth - edgePadding);
        worldPos.y = Mathf.Clamp(worldPos.y, camPos.y - camHeight + edgePadding, camPos.y + camHeight - edgePadding);
        return worldPos;
    }

    /// <summary>
    /// Finds where the ray from origin -> target intersects the camera's viewport bounds,
    /// preserving direction. If target is already inside bounds, returns target unchanged.
    /// </summary>
    private Vector3 ClampAlongRayToCameraView(Vector3 origin, Vector3 target)
    {
        float camHeight = mainCamera.orthographicSize;
        //float camWidth = camHeight * mainCamera.aspect;
        float camWidth = camHeight * (float)(16.0 / 9.0);

        Vector3 camPos = mainCamera.transform.position;

        float minX = camPos.x - camWidth + edgePadding;
        float maxX = camPos.x + camWidth - edgePadding;
        float minY = camPos.y - camHeight + edgePadding;
        float maxY = camPos.y + camHeight - edgePadding;

        Vector3 dir = target - origin;

        float t = 1f; // t=1 means "use the full target distance" (i.e. no clamp needed)

        if (Mathf.Abs(dir.x) > 0.0001f)
        {
            float tx1 = (minX - origin.x) / dir.x;
            float tx2 = (maxX - origin.x) / dir.x;
            if (tx1 > 0f) t = Mathf.Min(t, tx1);
            if (tx2 > 0f) t = Mathf.Min(t, tx2);
        }

        if (Mathf.Abs(dir.y) > 0.0001f)
        {
            float ty1 = (minY - origin.y) / dir.y;
            float ty2 = (maxY - origin.y) / dir.y;
            if (ty1 > 0f) t = Mathf.Min(t, ty1);
            if (ty2 > 0f) t = Mathf.Min(t, ty2);
        }

        t = Mathf.Clamp01(t);
        return origin + dir * t;
    }


    public void SetCrosshairColor(Color crosshairColor)
    {
        Crosshair.GetComponent<SpriteRenderer>().color = crosshairColor;
    }

    private void SetCrosshairDistance(float distance)
    {
        //Crosshair.transform.localPosition = new Vector3(distance, Crosshair.transform.localPosition.y, Crosshair.transform.localPosition.z);

        //clamp crosshair within camera view bound
        //Vector3 clampedWorldPos = ClampToCameraView(Crosshair.transform.position);
        //Crosshair.transform.position = clampedWorldPos;
        Vector3 offset = getAimDirection().normalized * distance;
        Vector3 targetPos = transform.position + offset;
        //Crosshair.transform.position = ClampToCameraView(targetPos);
        Crosshair.transform.position = ClampAlongRayToCameraView(transform.position, targetPos);
    }




    private void Start()
    {
        SetCrosshairDistance(CrosshairDistance);
        //to avoid multiple controller conflict //edit
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


    public Vector2 getAimDirection()
    {
        return aimDirection;
    }


    void OnAim(InputValue value) // getting controller aim stick direction
    {
        if (value.Get<Vector2>() != Vector2.zero)//dont include stick move back
        {
            //isAimStickUsed = true;
            aimDirection = value.Get<Vector2>();
            SetCrosshairDistance(CrosshairDistance);
            //if (inverseCrosshairDistance)
            //    SetCrosshairDistance(CrosshairMinDistance);
            //else
            //    SetCrosshairDistance(CrosshairMaxDistance);
        }
        //else
        //{
        //    isAimStickUsed = false;
        //    SetCrosshairDistance(CrosshairMinDistance);
        //    //if (inverseCrosshairDistance)
        //    //    SetCrosshairDistance(CrosshairMaxDistance);
        //    //else
        //    //    SetCrosshairDistance(CrosshairMinDistance);
        //}
    }

    private void Aim()
    {
        //mouse aiming
        if (currentMouse!=null)//edit    
        {
            mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //edit       
            //Vector3 aimDirection = (mouseCursorPos - transform.position).normalized; //edit
            aimDirection = (mouseCursorPos - transform.position).normalized; //edit
        }

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
        Crosshair.transform.rotation = Quaternion.identity;
        SetCrosshairDistance(CrosshairDistance);
        //if (!isAimStickUsed) 
        //    SetCrosshairDistance(CrosshairMinDistance);

    }
}
