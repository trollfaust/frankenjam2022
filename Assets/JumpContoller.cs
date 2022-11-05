using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetection))]
public class JumpContoller : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] float maxJumpHeight = 5f;
    [SerializeField] [Range(0.2f, 2.5f)] float timeToJumpApax;
    [SerializeField] float gravityMultiplierUpwards = 1f;
    [SerializeField] float gravityMultiplierDownwards = 6f;
    [SerializeField] float maxDropSpeed;
    [Header("Air Jumps")]
    [SerializeField] bool airJumpsActive;
    [SerializeField] int maxAirJumps = 0;
    [Header("Variable Jump Height (let go Jump Button)")]
    [SerializeField] bool variableJumpHeight;
    [SerializeField] float gravityMultiplierCutOff;
    [Header("Coyote Time")]
    [SerializeField] float coyoteTime = 0.15f;
    [Header("Jump Buffer before hit the Ground")]
    [SerializeField] float jumpBufferTime = 0.15f;

    private Rigidbody2D rb;
    private GroundDetection groundDetection;

    private float jumpSpeed;
    private float defaultGravityScale;
    private float gravityMultiplier = 1;
    private bool desiredJump;
    private bool isPressingJump;
    private bool onGround;
    private float jumpBufferCounter = 0f;
    private bool isJumping;
    private float coyoteTimeCounter = 0f;
    private Vector2 velocity;
    private int jumpCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetection = GetComponent<GroundDetection>();
        defaultGravityScale = 1f;
    }

    public void OnJump(bool keyIsDown)
    {
        if (keyIsDown)
        {
            desiredJump = true;
            isPressingJump = true;
        } else
        {
            isPressingJump = false;
        }
    }

    private void Update()
    {
        SetGravityScale();

        onGround = groundDetection.GetOnGround();

        if (jumpBufferTime > 0)
        {
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBufferTime)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0f;
                }
            }
        }

        if (!isJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        } else
        {
            coyoteTimeCounter = 0f;
        }

        if (onGround)
        {
            jumpCount = 0;
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;

        if (desiredJump)
        {
            DoJump();
            rb.velocity = velocity;
            
            return;
        }

        CalcGravity();
    }

    private void SetGravityScale()
    {
        Vector2 newGravity = new Vector2(0, (-2 * maxJumpHeight) / (timeToJumpApax * timeToJumpApax));
        rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }
    
    private void CalcGravity()
    {
        if (rb.velocity.y > 0.01f)
        {
            if (onGround)
            {
                gravityMultiplier = defaultGravityScale;
            } else
            {
                if (variableJumpHeight)
                {
                    if (isPressingJump && isJumping)
                    {
                        gravityMultiplier = gravityMultiplierUpwards;
                    } else
                    {
                        gravityMultiplier = gravityMultiplierCutOff;
                    }
                } else
                {
                    gravityMultiplier = gravityMultiplierUpwards;
                }
            }
        } else if (rb.velocity.y < -0.01f)
        {
            if (onGround)
            {
                gravityMultiplier = defaultGravityScale;
            } else
            {
                gravityMultiplier = gravityMultiplierDownwards;
            }
        } else
        {
            if (onGround)
            {
                isJumping = false;
            }

            gravityMultiplier = defaultGravityScale;
        }

        rb.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -maxDropSpeed, 100));
    }

    private void DoJump()
    {
        if (onGround || coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime || jumpCount <= maxAirJumps && airJumpsActive)
        {
            desiredJump = false;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;

            jumpCount++;

            jumpSpeed = Mathf.Sqrt(-2 * Physics2D.gravity.y * rb.gravityScale * maxJumpHeight);
            
            /*
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            } else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
            }
            */
            
            velocity.y += jumpSpeed;
            isJumping = true;
        }

        if (jumpBufferTime == 0)
        {
            desiredJump = false;
        }
    }
}
