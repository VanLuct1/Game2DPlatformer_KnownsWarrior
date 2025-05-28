using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 moveInput;

    public float jumpForce = 5f;
    Damageable damageable;

    public AudioClip attackSound; // Gán trong Inspector
    public AudioClip jumpSound;   // Gán trong Inspector
    public AudioClip gameOverSound; // Gán trong Inspector
    private AudioSource audioSource;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    return moveSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    bool _isMoving = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
            anm.SetBool("isMoving", value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get { return anm.GetBool(AnimationStrings.canMove); }
    }

    public bool IsAlive
    {
        get { return anm.GetBool(AnimationStrings.isAlive); }
    }

    Rigidbody2D rb;
    Animator anm;
    TouchingDirections touchingDirections;
    private bool wasAlive = true; // Theo dõi trạng thái sống

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        anm.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        // Khi nhân vật chết, phát âm thanh game over
        if (wasAlive && !IsAlive)
        {
            SoundManager soundManager = FindObjectOfType<SoundManager>();
            if (soundManager != null && gameOverSound != null)
            {
                soundManager.PlayGameOverSound(gameOverSound);
            }
        }
        wasAlive = IsAlive;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            anm.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anm.SetTrigger(AnimationStrings.attackTrigger);
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}