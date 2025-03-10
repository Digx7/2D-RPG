using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement3D : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float jumpBufferCheckDistance = 2f;
    public float groundedDistanceCheck = 2f;

    private Rigidbody rb;
    private Vector2 desiredMoveDirection;
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
        rb = gameObject.GetComponent<Rigidbody>();
        desiredMoveDirection = new Vector2();
    }

    
    public void FixedUpdate()
    {
        if(wantsToJump) Jump();
        Move();
    }

    private void Move()
    {
        Vector3 startingVelocity = rb.linearVelocity;

        Vector3 finalVelocity = new Vector3(desiredMoveDirection.y, 0, -desiredMoveDirection.x);
        finalVelocity = finalVelocity * moveSpeed * Time.deltaTime;

        finalVelocity = transform.TransformDirection(finalVelocity);

        finalVelocity.y = startingVelocity.y;


        rb.linearVelocity = finalVelocity;
    }

    private void Jump()
    {
        if(!isNearGround())
        {
            Debug.Log("Movement3D: Jump failed, not near ground");
            wantsToJump = false;
        }
        else if (isGrounded())
        {
            Debug.Log("Movement3D: Jumping");
            wantsToJump = false;
            Vector3 liftForce = new Vector3(0, jumpForce, 0);

            rb.AddForce(liftForce, ForceMode.VelocityChange);
            return;
        }
        Debug.Log("Movement3D: Jump waiting until closer to ground");
    }

    private bool isNearGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, jumpBufferCheckDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
            return true;
        }
        return false;
    }  

    private bool isGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundedDistanceCheck))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
            return true;
        }
        return false;
    }
    
}
