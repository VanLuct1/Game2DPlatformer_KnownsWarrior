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
    public float CurrentMoveSpeed{ get
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
            } else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    bool _isMoving = false;

    public bool IsMoving { get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            anm.SetBool("isMoving", value);
        }
    }

    //
    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRight = value;
        }
    }
    //
    public bool CanMove { get
        {
            return anm.GetBool(AnimationStrings.canMove);
        }
    }
    //Điều chỉnh damage
    public bool IsAlive
    {
        get
        {
            return anm.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator anm;
    TouchingDirections touchingDirections;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }


    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        anm.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    //ham di chuyen walk
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
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    // Hàm nhảy
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            anm.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anm.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    // Update the OnHit method to use the private setter of LockVelocity.
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }//
}
