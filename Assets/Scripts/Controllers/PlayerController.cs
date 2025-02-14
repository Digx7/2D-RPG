using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GameController
{
    [SerializeField] protected CameraManager cameraManager;
    [SerializeField] private UIWidgetDataChannel RequestLoadUIWidgetData;
    [SerializeField] private UIWidgetData activeTimeLoreWidgetData;
    private PlayerCharacter possessedPlayer;

    // OVERRIDE FUNCTIONS ==============================================

    public override bool PossessCharacter(Character newCharacter)
    {
        possessedPlayer = newCharacter as PlayerCharacter;

        return base.PossessCharacter(newCharacter);
    }

    public override void ForcePossessCharacter(Character newCharacter)
    {
        possessedPlayer = newCharacter as PlayerCharacter;

        base.ForcePossessCharacter(newCharacter);
    }

    // CAMERA FUNCTIONS ===================================================

    public bool ConnectCameraManager(CameraManager newCameraManager)
    {
        if(!IsCameraManagerValid(newCameraManager)) return false;

        if(newCameraManager == cameraManager) return true;
        
        if(cameraManager != null)
        {
            Debug.LogWarning("The CameraManager: " + newCameraManager + " tried to connect to the PlayerController " + this + " but it is already connected to the CameraManager: " + cameraManager + ".  If this was intentional use ForceConnectCameraManager instead");
            return false;
        }

        cameraManager = newCameraManager;
        cameraManager.SetID(ID);
        return true;
    }

    public void ForceConnectCameraManager(CameraManager newCameraManager)
    {
        if(!IsCameraManagerValid(newCameraManager))return;
        if(newCameraManager == cameraManager)return;

        cameraManager = newCameraManager;
        cameraManager.SetID(ID);
    }

    private bool IsCameraManagerValid(CameraManager newCameraManager)
    {
        if(newCameraManager == null) return false;
        else return true;
    }

    // PLAYER INPUT FUNCTIONS =============================================

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        
        // The direction the player is inputing on the keyboard or gamepad
        Vector2 direction = callbackContext.ReadValue<Vector2>();
        
        // For more on the InputActionPhase see: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (callbackContext.phase)
        {
            case InputActionPhase.Disabled:
                // Add Code here
                break;
            case InputActionPhase.Waiting:
                // Add Code here
                break;
            case InputActionPhase.Started:
                // Add Code here
                possessedPlayer.UpdateDesiredMoveDirection(direction);
                break;
            case InputActionPhase.Performed:
                // Add Code here
                possessedPlayer.UpdateDesiredMoveDirection(direction);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                possessedPlayer.UpdateDesiredMoveDirection(new Vector2(0,0));
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        
        // For more on the InputActionPhase see: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (callbackContext.phase)
        {
            case InputActionPhase.Disabled:
                // Add Code here
                break;
            case InputActionPhase.Waiting:
                // Add Code here
                break;
            case InputActionPhase.Started:
                // Add Code here
                break;
            case InputActionPhase.Performed:
                // Add Code here
                possessedPlayer.Jump();
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnFire1(InputAction.CallbackContext callbackContext)
    {
        
        // For more on the InputActionPhase see: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (callbackContext.phase)
        {
            case InputActionPhase.Disabled:
                // Add Code here
                break;
            case InputActionPhase.Waiting:
                // Add Code here
                break;
            case InputActionPhase.Started:
                // Add Code here
                break;
            case InputActionPhase.Performed:
                // Add Code here
                possessedPlayer.Fire1();
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnFire2(InputAction.CallbackContext callbackContext)
    {
        
        // For more on the InputActionPhase see: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (callbackContext.phase)
        {
            case InputActionPhase.Disabled:
                // Add Code here
                break;
            case InputActionPhase.Waiting:
                // Add Code here
                break;
            case InputActionPhase.Started:
                // Add Code here
                break;
            case InputActionPhase.Performed:
                // Add Code here
                possessedPlayer.Fire2();
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnLore(InputAction.CallbackContext callbackContext)
    {
        
        // For more on the InputActionPhase see: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionPhase.html
        switch (callbackContext.phase)
        {
            case InputActionPhase.Disabled:
                // Add Code here
                break;
            case InputActionPhase.Waiting:
                // Add Code here
                break;
            case InputActionPhase.Started:
                // Add Code here
                break;
            case InputActionPhase.Performed:
                // Add Code here
                RequestLoadUIWidgetData.Raise(activeTimeLoreWidgetData);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnDeviceLost(PlayerInput playerInput)
    {

    }

    public void OnDeviceRegained(PlayerInput playerInput)
    {

    }

    public void OnControlsChanged(PlayerInput playerInput)
    {

    }
}
