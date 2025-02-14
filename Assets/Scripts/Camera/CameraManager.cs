using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] protected int ID = 1;
    [SerializeField] PlayerController connectedPlayerController;
    [SerializeField] PlayerCharacter playerCharacter;

    [SerializeField] protected IntChannel OnCameraManagerFinishedSetup;

    [SerializeField] private bool runSetupOnEnable = true;
    [SerializeField] private PlayerController controllerToConnectToOnEnable;
    [SerializeField] private PlayerCharacter playerCharacterToConnectToOnEnable;

    // CONNECTING TO PLAYER AND CONTROLLERS =========================================

    public bool ConnectToPlayerController(PlayerController newPlayerController)
    {
        if(!IsPlayerControllerValid(newPlayerController)) return false;

        if(newPlayerController == connectedPlayerController) return true;

        if(newPlayerController.ConnectCameraManager(this))
        {
            connectedPlayerController = newPlayerController;
            return true;
        }

        Debug.LogWarning("The CameraManager: " + this + " failed to connect to PlayerController, because it is connecte to another CameraManager.  If this was intentional use ForceConnectToPlayerController instead");

        return false;
    }

    public void ForceConnectToPlayerController(PlayerController newPlayerController)
    {
        if(!IsPlayerControllerValid(newPlayerController)) return;
        if(newPlayerController == connectedPlayerController) return;

        newPlayerController.ForceConnectCameraManager(this);
        connectedPlayerController = newPlayerController;
    }

    public bool ConnectToPlayerCharacter(PlayerCharacter newPlayerCharacter)
    {
        if(!IsPlayerCharacterValid(newPlayerCharacter)) return false;

        if(newPlayerCharacter == playerCharacter) return true;

        if(newPlayerCharacter.ConnectCameraManager(this))
        {
            playerCharacter = newPlayerCharacter;
            return true;
        }

        Debug.LogWarning("The CameraManager: " + this + " failed to connect to PlayerController, because it is connecte to another CameraManager.  If this was intentional use ForceConnectToPlayerController instead");

        return false;
    }

    public void ForceConnectToPlayerCharacter(PlayerCharacter newPlayerCharacter)
    {
        if(!IsPlayerCharacterValid(newPlayerCharacter)) return;
        if(newPlayerCharacter == playerCharacter) return;
        
        newPlayerCharacter.ForceConnectCameraManager(this);
        playerCharacter = newPlayerCharacter;
    }

    public void SetID(int newID)
    {
        if(ID == newID) return;
        
        ID = newID;
        if(IsPlayerControllerValid(connectedPlayerController)) connectedPlayerController.SetID(ID);
        if(IsPlayerCharacterValid(playerCharacter)) playerCharacter.SetID(ID);
    }

    private bool IsPlayerControllerValid(PlayerController playerController)
    {
        if(playerController == null) return false;
        else return true;
    }

    private bool IsPlayerCharacterValid(PlayerCharacter playerCharacter)
    {
        if(playerCharacter == null) return false;
        else return true;
    }

    // SETUP AND TEARDOWN ===================================================================

    protected virtual void OnEnable()
    {
        if(runSetupOnEnable)Setup(ID, controllerToConnectToOnEnable, playerCharacterToConnectToOnEnable);
    }

    protected virtual void OnDisable()
    {
        Teardown();
    }

    public virtual void Setup(int newID = 1, PlayerController controllerToConnectTo = null, PlayerCharacter newPlayerCharacter = null)
    {
        SetID(newID);
        ConnectToPlayerController(controllerToConnectTo);
        ConnectToPlayerCharacter(newPlayerCharacter);
        OnCameraManagerFinishedSetup.Raise(ID);
    }

    protected virtual void Teardown()
    {

    }

}
