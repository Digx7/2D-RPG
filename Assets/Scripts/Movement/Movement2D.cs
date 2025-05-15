using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    public float moveSpeed;
    public bool isFlying = false;
    public float jumpForce;
    public float jumpBufferCheckDistance = 2f;
    public float groundedCheckDistance = 2f;
    public ContactFilter2D jumpBufferContactFilter;
    public ContactFilter2D groundedContactFilter;
    public BooleanEvent OnAirborn;
    public BooleanEvent OnFalling;

    private Rigidbody2D rb;
    private Vector2 desiredMoveDirection;

    private bool isAirborn = false;
    private bool isFalling = false;

    public void setDesiredMoveDirection(Vector2 newDirection)
    {
        desiredMoveDirection = newDirection;
    }
    private bool wantsToJump = false;
    public void tryToJump()
    {
        wantsToJump = true;
    }

    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        desiredMoveDirection = new Vector2();
    }

    
    public void FixedUpdate()
    {
        if(wantsToJump && !isFlying) Jump();
        if(!isFlying) RefreshAirBornState();
        Move();
    }

    // public void Update()
    // {
    //     if(wantsToJump && !isFlying) Jump();
    //     if(!isFlying) RefreshAirBornState();
    //     Move();
    // }

    private void Move()
    {
        Vector3 startingVelocity = rb.linearVelocity;

        Vector3 finalVelocity = new Vector3(desiredMoveDirection.x, 0, 0);

        if(isFlying)
        {
            finalVelocity.y = desiredMoveDirection.y;
        }

        
        finalVelocity = finalVelocity * moveSpeed * Time.deltaTime;

        finalVelocity = transform.TransformDirection(finalVelocity);

        if(!isFlying)
        {
            finalVelocity.y = startingVelocity.y;
        }


        rb.linearVelocity = finalVelocity;
    }

    private void Jump()
    {
        if(!checkIfCloseEnoughForJumpBuffer())
        {
            Debug.Log("Movement2D: Jump failed, not near ground");
            wantsToJump = false;
        }
        else if (checkIfIsGrounded())
        {
            Debug.Log("Movement2D: Jumping");
            wantsToJump = false;
            Vector2 liftForce = new Vector3(0, jumpForce);


            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;

            rb.AddForce(liftForce, ForceMode2D.Impulse);
            return;
        }
        Debug.Log("Movement2D: Jump waiting until closer to ground");
    }

    private void RefreshAirBornState()
    {
        bool ogIsAirborn = isAirborn;
        bool ogIsFalling = isFalling;

        isAirborn = !checkIfIsGrounded();
        if(isAirborn && rb.linearVelocity.y < 0) isFalling = true;
        else isFalling = false;

        if(ogIsAirborn != isAirborn) OnAirborn.Invoke(isAirborn);
        if(ogIsFalling != isFalling) OnFalling.Invoke(isAirborn);
    }

    private bool checkIfCloseEnoughForJumpBuffer()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = transform.TransformDirection(Vector2.down);
        
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Physics2D.Raycast(origin, direction, jumpBufferContactFilter, hits, jumpBufferCheckDistance);

        if(hits.Count > 0)
        {
            // Debug.Log("Movement2D: isNearGround() hit something");
            return true;
        }
        return false;
    }  

    private bool checkIfIsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = transform.TransformDirection(Vector2.down);
        
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        Physics2D.Raycast(origin, direction, groundedContactFilter, hits, groundedCheckDistance);
        
        if(hits.Count > 0)
        {
            // Debug.Log("Movement2D: isGrounded() hit something");
            return true;
        }

        return false;
    }
    
}
