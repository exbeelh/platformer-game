using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

 
    private int facingDirection = 1;
 
    private bool isFacingRight = true;
    private bool isWalking;

    private bool isWallSliding;
    private bool canWallJump;

    private bool canMove;
    private bool canFlip;
    private bool isDashing;
    private bool knockback;

    public bool isJumping;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]
    private Vector2 knockbackSpeed;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public LayerMask whatIsGround;

    private Collision col;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;

    public float turnTimerSet = 0.1f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    public float wallJumpLerp = 10;

    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collision>();
    }

    void Update()
    {
        ChcekInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfWallSliding();
        CheckDash();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckIfWallSliding()
    {
        if(col.onWall && movementInputDirection == facingDirection && !col.onGround && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", col.onGround);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void ChcekInput()
    {
        movementInputDirection = CrossPlatformInputManager.GetAxis("Horizontal");

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if(col.onGround)
            {
                NormalJump(Vector2.up, false);
            }
            if(col.onWall && !col.onGround)
            {
                WallJump();
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("Horizontal") && col.onWall)
        {
            if (!col.onGround && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
            isWallSliding = false;
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("Dash")) 
        {
            if(Time.time >= (lastDash + dashCooldown))
                AttempToDash();
        }

        if (!canWallJump)
        {
            rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(movementInputDirection * movementSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }

    }

    private void AttempToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
        {
            SoundManager.PlaySound("dash");
        }

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || col.onWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void NormalJump(Vector2 dir, bool wall)
    {
        isJumping = true;
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        particle.Play();
    }

    private void WallJump()
    {
        if ((facingDirection == 1 && col.onRightWall) || facingDirection == -1 && !col.onRightWall)
        {
            facingDirection *= 1;
            Flip();
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = col.onRightWall ? Vector2.left : Vector2.right;

        NormalJump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        canWallJump = true;
    }

    private void ApplyMovement()
    {
        if (!col.onGround && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    int ParticleSide()
    {
        int particleSide = col.onRightWall ? 1 : -1;
        return particleSide;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

}
