using UnityEngine;

[RequireComponent(typeof(Movement2D))]
public class PlayerCharacter2D : PlayerCharacter
{
    private Movement2D movement2D;
    private Animator animator;
    private float lastMoveDirection;
    [SerializeField] private GameObject playerSprite;

    protected override void OnDisable()
    {
        movement2D.OnAirborn.RemoveListener(UpdateAnimatorAirborn);
        movement2D.OnFalling.RemoveListener(UpdateAnimatorFalling);
        
        base.OnDisable();
    }

    protected override void Awake()
    {
        movement2D = gameObject.GetComponent<Movement2D>();

        movement2D.OnAirborn.AddListener(UpdateAnimatorAirborn);
        movement2D.OnFalling.AddListener(UpdateAnimatorFalling);


        animator = playerSprite.GetComponent<Animator>();
        lastMoveDirection = 1f;
    }

    public override void UpdateDesiredMoveDirection(Vector2 newDesiredDirection)
    {
        // Debug.Log("PlayerCharacter3D: UpdateDesiredMoveDirection( " + newDesiredDirection + ")");
        base.UpdateDesiredMoveDirection(newDesiredDirection);
        movement2D.setDesiredMoveDirection(desiredMoveDirection);

        UpdateRunningAnimation(newDesiredDirection);
        UpdatePlayerSpriteRotation(newDesiredDirection);
    }

    private void UpdateRunningAnimation(Vector2 moveDirection)
    {
        if(moveDirection.x >= 0.1 || moveDirection.x <= -0.1)
        {
            if(animator != null) animator.SetBool("IsRunning", true);
        }
        else 
        {
            if(animator != null) animator.SetBool("IsRunning", false);
        }
    }

    private void UpdatePlayerSpriteRotation(Vector2 moveDirection)
    {
        if(moveDirection.x >= 0.1 || moveDirection.x <= -0.1)
        {
            lastMoveDirection = moveDirection.x;
        }
        
        if(lastMoveDirection > 0)
        {
            // Face right
            Vector3 newRotation = playerSprite.transform.localEulerAngles;
            newRotation.y = 0f;
            playerSprite.transform.localEulerAngles = newRotation;
        }
        else
        {
            // Face left
            Vector3 newRotation = playerSprite.transform.localEulerAngles;
            newRotation.y = 180f;
            playerSprite.transform.localEulerAngles = newRotation;
        }
    }

    public void UpdateAnimatorAirborn(bool value)
    {
        if(animator != null) animator.SetBool("IsAirborn", value);
    }

    public void UpdateAnimatorFalling(bool value)
    {
        if(animator != null) animator.SetBool("IsFalling", value);
    }

    public void UpdateAnimatorInCombat(bool value)
    {
        if(animator != null) animator.SetBool("InCombat", value);
    }

    public override void Jump()
    {
        Debug.Log("PlayerCharacter3D: Jump()");
        base.Jump();
        movement2D.tryToJump();
    }
}