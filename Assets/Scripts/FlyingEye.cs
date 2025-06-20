using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flySpeed = 2f;
    private float waypointReachedDistance = 0.1f;

    public DirectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;


    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    private int waypointIndex = 0;

    private bool _hasTarget = false;

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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            nextWaypoint = waypoints[waypointIndex];
        }
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath); // 
    }

    private void Update()
    {
        if (biteDetectionZone != null && biteDetectionZone.directedColliders != null)
        {
            HasTarget = biteDetectionZone.directedColliders.Count > 0;
        }
        else
        {
            HasTarget = false;
        }

    }


    private void FixedUpdate()
    {
        if (damageable != null && damageable.IsAlive)
        {
            if (CanMove)
            {
                Fly();
            }
            else
            {
                rb.velocity = Vector2.zero; // ngừng di chuyển
            }
        }
    }

    private void Fly()
    {
        if (nextWaypoint == null) return;

        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flySpeed;

        UpdateDirection();

        if (distance <= waypointReachedDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
            nextWaypoint = waypoints[waypointIndex];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;

        if (rb.velocity.x < 0 && localScale.x > 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
        else if (rb.velocity.x > 0 && localScale.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y); // Ngừng di chuyển ngang
        deathCollider.enabled = true; // Kích hoạt va chạm khi chết
    }
}
