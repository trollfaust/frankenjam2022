using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetection))]
public class MovementController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float friction;
    [Header("Acceleration")]
    [SerializeField] bool useAcceleration = false;
    [SerializeField] float maxAccelerationOnGround = 50f;
    [SerializeField] float maxAccelerationInAir = 40f;
    [SerializeField] float maxDecelerationOnGround = 50f;
    [SerializeField] float maxDecelerationInAir = 40f;
    [SerializeField] float maxTurnSpeedOnGround = 80f;
    [SerializeField] float maxTurnSpeedInAir = 80f;

    private Rigidbody2D rb;
    private GroundDetection groundDetection;
    private Vector2 velocity;
    private Vector2 desiredVelocity;
    private bool onGround;
    private bool isPressingKey;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    private float directionX;
    private float maxSpeedChange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetection = GetComponent<GroundDetection>();
    }

    /// <summary>
    /// Call to move Character on Input. HorizontalMovmentValue from a Input (-1 to 1)
    /// </summary>
    /// <param name="horizontalMovementValue"></param>
    public void OnMovement(float horizontalMovementValue)
    {
        directionX = horizontalMovementValue;
    }

    private void Update()
    {
        if (directionX != 0)
        {
            transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
            isPressingKey = true;
        } else
        {
            isPressingKey = false;
        }

        desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        onGround = groundDetection.GetOnGround();

        velocity = rb.velocity;

        if (useAcceleration)
        {
            MoveWithAcceleration();
        } else
        {
            if (onGround)
            {
                MoveWithoutAcceleration();
            } else
            {
                MoveWithAcceleration();
            }
        }
    }

    #region Move Funktions
    private void MoveWithAcceleration()
    {
        acceleration = (onGround) ? maxAccelerationOnGround : maxAccelerationInAir;
        deceleration = (onGround) ? maxDecelerationOnGround : maxDecelerationInAir;
        turnSpeed = (onGround) ? maxTurnSpeedOnGround : maxTurnSpeedInAir;

        if (isPressingKey)
        {
            // We want to Turn Around
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.fixedDeltaTime;
            } else
            {
                maxSpeedChange = acceleration * Time.fixedDeltaTime;
            }
        } else
        {
            maxSpeedChange = deceleration * Time.fixedDeltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        rb.velocity = velocity;
    }

    private void MoveWithoutAcceleration()
    {
        velocity.x = desiredVelocity.x;

        rb.velocity = velocity;
    }
    #endregion
}
