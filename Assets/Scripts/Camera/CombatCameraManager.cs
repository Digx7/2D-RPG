using UnityEngine;

public class CombatCameraManager : CameraManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Setup(int newID = 1, PlayerController controllerToConnectTo = null, PlayerCharacter newPlayerCharacter = null)
    {
        base.Setup(newID, controllerToConnectTo, newPlayerCharacter);
    }

    protected override void Teardown()
    {

    }

}