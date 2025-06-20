using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    // 
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.1f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator anm;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    //Kiểm tra xem có va chạm với mặt đất hay không
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
    //Kiểm tra xem có va chạm với tường hay không
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
    //Kiểm tra xem có va chạm với trần hay không
    [SerializeField]
    bool _isOnCeiling;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

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

    public Vector2 WalkDirectionVector { get; internal set; }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

    }
}