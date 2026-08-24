using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlatformDrop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Drop Settings")]
    [SerializeField] private float dropDuration = 0.25f;
    [SerializeField] private float downDeadzone = 0.5f;
    [SerializeField] private LayerMask oneWayPlatformLayer;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private Vector2 checkOffset = new Vector2(0f, -0.1f);

    private void OnEnable()
    {
        jumpAction.action.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        float verticalInput = moveAction.action.ReadValue<Vector2>().y;

        if (verticalInput < -downDeadzone)
        {
            TryDropThrough();
        }
        // else: let your normal jump logic handle it elsewhere
    }

    private void TryDropThrough()
    {
        Vector2 checkPos = (Vector2)transform.position + checkOffset;
        Collider2D platform = Physics2D.OverlapCircle(checkPos, checkRadius, oneWayPlatformLayer);

        if (platform != null)
        {
            StartCoroutine(DropThroughRoutine(platform));
        }
    }

    private IEnumerator DropThroughRoutine(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        yield return new WaitForSeconds(dropDuration);

        // Safety check in case platform was destroyed mid-drop
        if (platform != null)
            Physics2D.IgnoreCollision(playerCollider, platform, false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + checkOffset, checkRadius);
    }
}
