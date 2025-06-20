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

    // Mana
    public int maxMana = 100;
    public int currentMana;
    public int manaCostPerFireball = 5;

    Damageable damageable;
    Rigidbody2D rb;
    Animator anm;
    TouchingDirections touchingDirections;

    private Vector3 spawnPosition; // Vị trí xuất phát
    private bool isRespawning = false;

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

    [SerializeField] bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            _isMoving = value;
            anm.SetBool("isMoving", value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove => anm.GetBool(AnimationStrings.canMove);
    public bool IsAlive => anm.GetBool(AnimationStrings.isAlive);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        currentMana = maxMana;

        spawnPosition = transform.position; // Lưu vị trí ban đầu
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        anm.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    private void Update()
    {
        // Nếu người chơi chết và chưa đang hồi sinh → chạy coroutine
        if (!IsAlive && !isRespawning)
        {
            StartCoroutine(RespawnAfterDelay(5f));
        }
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        isRespawning = true;
        yield return new WaitForSeconds(delay);

        transform.position = spawnPosition;
        damageable.Health = damageable.MaxHealth;
        currentMana = maxMana;

        anm.SetBool(AnimationStrings.isAlive, true);
        isRespawning = false;
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
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started && currentMana >= manaCostPerFireball)
        {
            currentMana -= manaCostPerFireball;
            anm.SetBool(AnimationStrings.isRangedAttacking, true);
            // UIManager.Instance.UpdateMana(currentMana, maxMana);
        }
    }

    public void ResetRangedAttack()
    {
        anm.SetBool(AnimationStrings.isRangedAttacking, false);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anm.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
        }
    }

    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        Debug.Log("Đã hồi mana: " + amount);
        // UIManager.Instance.UpdateMana(currentMana, maxMana);
    }
}
