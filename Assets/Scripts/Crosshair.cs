using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    Vector2 mouseCursorPos;  
    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {      
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mouseCursorPos;
    }
}
