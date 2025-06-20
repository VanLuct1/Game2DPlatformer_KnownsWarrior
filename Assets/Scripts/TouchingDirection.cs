using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.1f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator anm;
    [HideInInspector]
    public Vector2 WalkDirectionVector = Vector2.right;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    bool _isGrounded;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            anm.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    bool _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            anm.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    bool _isOnCeiling;

    private Vector2 wallCheckDirection => WalkDirectionVector;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            anm.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        anm = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        int wallHitCount = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance);
        bool foundValidWall = false;
        for (int i = 0; i < wallHitCount; i++)
        {
            if (!wallHits[i].collider.CompareTag("Player") && !wallHits[i].collider.CompareTag("Skeleton"))
            {
                foundValidWall = true;
                break;
            }
        }
        IsOnWall = foundValidWall;

        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}