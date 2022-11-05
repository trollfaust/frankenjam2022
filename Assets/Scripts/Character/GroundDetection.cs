using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    private bool onGround;

    [SerializeField] private float groundLength = 0.95f;
    [SerializeField] private Vector3 colliderOffset;

    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        onGround = hit1 || hit2;

        if (onGround)
        {
            if (hit1)
            {
                transform.parent = hit1.transform;
            } else if (hit2)
            {
                transform.parent = hit2.transform;
            }
        } else
        {
            transform.parent = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    public bool GetOnGround() { return onGround; }
}
