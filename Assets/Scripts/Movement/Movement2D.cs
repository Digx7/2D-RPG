using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement2D : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float jumpBufferCheckDistance = 2f;
    public float groundedDistanceCheck = 2f;

    private Rigidbody2D rb;
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
        rb = gameObject.GetComponent<Rigidbody2D>();
        desiredMoveDirection = new Vector2();
    }

    
    public void FixedUpdate()
    {
        if(wantsToJump) Jump();
        Move();
    }

    private void Move()
    {
        // TODO
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
            Vector2 liftForce = new Vector3(0, jumpForce);

            rb.AddForce(liftForce, ForceMode2D.Impulse);
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
