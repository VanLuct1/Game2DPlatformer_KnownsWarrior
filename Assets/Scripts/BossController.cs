using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class BossController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float walkStopRate = 0.05f;
    public DirectionZone attackZone;
    public DirectionZone cliffDetectionZone;

    public float skillInterval = 10f;
    private float skillTimer = 0f;

    public GameObject stompEffectPrefab;

    [Header("Summon Settings")]
    public GameObject skeletonPrefab; // Prefab quái người xương
    public Transform[] summonPoints;  // Các điểm sinh quái
    private bool hasSummoned = false; // Đảm bảo chỉ gọi 1 lần

    private Transform playerTransform;
    private Damageable damageable;

    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    Animator animator;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                WalkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
                transform.localScale = new Vector3(value == WalkableDirection.Right ? -1 : 1, transform.localScale.y, transform.localScale.z);
            }
            _walkDirection = value;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        WalkDirection = WalkableDirection.Left;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get => _hasTarget;
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCooldown);
        private set => animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    void Update()
    {
        HasTarget = attackZone.directedColliders.Count > 0;

        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;

        skillTimer += Time.deltaTime;
        if (skillTimer >= skillInterval)
        {
            skillTimer = 0f;
            TriggerStomp();
        }

        // Kiểm tra máu và spawn Skeleton
        if (damageable != null && damageable.Health <= damageable.MaxHealth * 0.5f && !hasSummoned)
        {
            SpawnSkeletons();
            hasSummoned = true; // Đánh dấu đã summon
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Manually triggering stomp!");
            SpawnStompEffect();
        }
    }

    void FixedUpdate()
    {
        if (touchingDirection.IsOnWall && touchingDirection.IsGrounded)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirection.IsGrounded)
                rb.velocity = new Vector2(moveSpeed * WalkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }

        touchingDirection.WalkDirectionVector = WalkDirectionVector;
    }

    private void FlipDirection()
    {
        WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }

    private void TriggerStomp()
    {
        Debug.Log("Triggering Stomp animation");
        animator.SetTrigger("Stomp");
    }

    public void SpawnStompEffect()
    {
        if (playerTransform != null && stompEffectPrefab != null)
        {
            Vector3 spawnPosition = playerTransform.position + new Vector3(0, 10f, 0);
            Debug.Log("Spawn fireball at: " + spawnPosition);
            Instantiate(stompEffectPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Player or prefab missing!");
        }
    }

    private void SpawnSkeletons()
    {
        if (skeletonPrefab != null && summonPoints != null && summonPoints.Length > 0)
        {
            foreach (Transform point in summonPoints)
            {
                if (point != null)
                {
                    Instantiate(skeletonPrefab, point.position, Quaternion.identity);
                    Debug.Log("Spawned Skeleton at: " + point.position);
                }
            }
        }
    }
}