using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Skeleton : MonoBehaviour
{
    public float moveSpeed = 2f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection;  }
        set { 
            if(_walkDirection != value)
            {
                //Direction flip
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right)
                {
                    WalkDirectionVector = Vector2.right;
                }
                else if(value == WalkableDirection.Left)
                {
                    WalkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }
    private void FixedUpdate()
    {
        if(touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }

        rb.velocity = new Vector2(moveSpeed * WalkDirectionVector.x, rb.velocity.y);
    }
    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Loi direction khong duoc tim thay");
        }
    }
}
