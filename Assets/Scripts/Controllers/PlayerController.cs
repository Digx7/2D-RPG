using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : GameController
{
    [SerializeField] protected CameraManager cameraManager;
    [SerializeField] private UIWidgetDataChannel RequestLoadUIWidgetData;
    [SerializeField] private UIWidgetData activeTimeLoreWidgetData;
    [SerializeField] private UIWidgetData pauseMenuWidgetData;
    [SerializeField] private Channel onCombatStartChannel;
    [SerializeField] private Channel onCombatEndChannel;
    [SerializeField] private BooleanChannel requestCanConfirmAbilities;
    private PlayerCharacter possessedPlayer;
    private PlayerInput playerInput;
    private List<CombatUnit> playerCombatUnits;
    private bool canConfirmAbilities = true;

    // OVERRIDE FUNCTIONS ==============================================

    public override void Setup(int newID = 1, Character characterToPossess = null)
    {
        playerCombatUnits = new List<CombatUnit>();
        
        base.Setup(newID, characterToPossess);
    }
    
    public override bool PossessCharacter(Character newCharacter)
    {
        if(newCharacter != null)
        {
            possessedPlayer = newCharacter as PlayerCharacter;
            // playerCombatUnit = possessedPlayer.gameObject.GetComponent<CombatUnit>();
        } 

        return base.PossessCharacter(newCharacter);
    }

    public override void ForcePossessCharacter(Character newCharacter)
    {
        if(newCharacter != null)
        {
            possessedPlayer = newCharacter as PlayerCharacter;
            // playerCombatUnit = possessedPlayer.gameObject.GetComponent<CombatUnit>();
        }

        base.ForcePossessCharacter(newCharacter);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        onCombatStartChannel.channelEvent.AddListener(OnCombatStart);
        onCombatEndChannel.channelEvent.AddListener(OnCombatEnd);
        requestCanConfirmAbilities.channelEvent.AddListener(SetCanConfirmAbilities);

        playerInput = GetComponent<PlayerInput>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        onCombatStartChannel.channelEvent.RemoveListener(OnCombatStart);
        onCombatEndChannel.channelEvent.RemoveListener(OnCombatEnd);
        requestCanConfirmAbilities.channelEvent.AddListener(SetCanConfirmAbilities);
    }

    // CHANNEL FUNCTIONS ================================================

    public void OnCombatStart()
    {
        Debug.Log("PlayerController: On Combat Start");

        // Clears any remaining playercombat unit references
        playerCombatUnits.Clear();

        // Gets all new playercombat unit references
        CombatUnit[] combatUnits = FindObjectsByType<CombatUnit>(FindObjectsSortMode.None);
        for (int i = 0; i < combatUnits.Length; i++)
        {
            if(combatUnits[i].combatFaction == CombatFaction.PLAYER) playerCombatUnits.Add(combatUnits[i]);
        }

        // Switches action map
        playerInput.SwitchCurrentActionMap("Combat");

        // Stops the player moving
        possessedPlayer.UpdateDesiredMoveDirection(new Vector2(0,0));

        // Updates player animations
        PlayerCharacter2D playerCharacter2D = (PlayerCharacter2D)possessedPlayer;
        playerCharacter2D.UpdateAnimatorInCombat(true);
    }
    public void OnCombatEnd()
    {
        Debug.Log("PlayerController: On Combat End");

        // Clears any remaining playercombat unit references
        playerCombatUnits.Clear();

        // Switches action map
        playerInput.SwitchCurrentActionMap("Exploration");
        
        // Updates player animations
        PlayerCharacter2D playerCharacter2D = (PlayerCharacter2D)possessedPlayer;
        playerCharacter2D.UpdateAnimatorInCombat(false);
    }

    public void SetCanConfirmAbilities(bool input)
    {
        canConfirmAbilities = input;
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

    // Exploration Inputs ---------------------------

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

    public void OnInteract_0(InputAction.CallbackContext callbackContext)
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
                possessedPlayer.Interact(0);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnInteract_1(InputAction.CallbackContext callbackContext)
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
                possessedPlayer.Interact(1);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnInteract_2(InputAction.CallbackContext callbackContext)
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
                possessedPlayer.Interact(2);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnInteract_3(InputAction.CallbackContext callbackContext)
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
                possessedPlayer.Interact(3);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnInteract_4(InputAction.CallbackContext callbackContext)
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
                possessedPlayer.Interact(4);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    // COMBAT INPUT FUNCTIONS ============================================

    public void OnAbility1(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(0);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility2(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(1);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility3(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(2);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility4(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(3);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility5(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(4);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility6(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(5);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility7(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(6);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility8(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(7);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility9(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(8);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility10(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(9);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility11(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(10);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnAbility12(InputAction.CallbackContext callbackContext)
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
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.PreviewAbility(11);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnConfirmAbility(InputAction.CallbackContext callbackContext)
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
                if(canConfirmAbilities)ConfirmAbility();
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnBackAbility(InputAction.CallbackContext callbackContext)
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
                if(canConfirmAbilities)
                {
                    foreach (CombatUnit unit in playerCombatUnits)
                    {
                        unit.StopPreviewing();
                    }
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnMousePosition(InputAction.CallbackContext callbackContext)
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
                AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
                abilityUsageContext.Setup();

                Vector2Control mouseScreenPos = Pointer.current.position;
                Vector3 point = new Vector3();

                point = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x.value, mouseScreenPos.y.value, 0));
                point.z = 0;

                abilityUsageContext.m_mousePos = point;
                
                foreach (CombatUnit unit in playerCombatUnits)
                {
                    unit.RenderPreviewSelection(abilityUsageContext);
                }
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    public void OnMoveCamera(InputAction.CallbackContext callbackContext)
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
                
                cameraManager.MoveCameraManually(callbackContext.ReadValue<Vector2>());

                break;
            case InputActionPhase.Canceled:
                // Add Code here

                cameraManager.MoveCameraManually(Vector2.zero);

                break;
            default:
                // Add Code here
                break;
        }
    }

    // Universal Inputs ---------------------------

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

    public void OnPause(InputAction.CallbackContext callbackContext)
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
                RequestLoadUIWidgetData.Raise(pauseMenuWidgetData);
                break;
            case InputActionPhase.Canceled:
                // Add Code here
                break;
            default:
                // Add Code here
                break;
        }
    }

    // System ---------------------------------------------------

    public void OnDeviceLost(PlayerInput playerInput)
    {

    }

    public void OnDeviceRegained(PlayerInput playerInput)
    {

    }

    public void OnControlsChanged(PlayerInput playerInput)
    {

    }


    // Other -------------------------------------------

    public void ConfirmAbility()
    {
        AbilityUsageContext abilityUsageContext = new AbilityUsageContext();
        abilityUsageContext.Setup();

        Vector2Control mouseScreenPos = Pointer.current.position;
        Vector3 point = new Vector3();

        point = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x.value, mouseScreenPos.y.value, 0));
        point.z = 0;

        abilityUsageContext.m_mousePos = point;
        
        foreach (CombatUnit unit in playerCombatUnits)
        {
            unit.ConfirmAbility(abilityUsageContext);
        }
    }

}
