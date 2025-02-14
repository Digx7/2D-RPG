using UnityEngine;

[RequireComponent(typeof(Movement3D))]
public class PlayerCharacter3D : PlayerCharacter
{
    private Movement3D movement3D;

    protected override void Awake()
    {
        movement3D = gameObject.GetComponent<Movement3D>();
    }

    public override void UpdateDesiredMoveDirection(Vector2 newDesiredDirection)
    {
        Debug.Log("PlayerCharacter3D: UpdateDesiredMoveDirection( " + newDesiredDirection + ")");
        base.UpdateDesiredMoveDirection(newDesiredDirection);
        movement3D.setDesiredMoveDirection(desiredMoveDirection);
    }

    public override void Jump()
    {
        Debug.Log("PlayerCharacter3D: Jump()");
        base.Jump();
        movement3D.tryToJump();
    }
}