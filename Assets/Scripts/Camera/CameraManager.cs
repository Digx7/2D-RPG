using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] protected int ID = 1;
    [SerializeField] public Camera camera;
    [SerializeField] public GameObject cameraPrefab;
    [SerializeField] PlayerController connectedPlayerController;
    [SerializeField] PlayerCharacter playerCharacter;

    [SerializeField] protected IntChannel OnCameraManagerFinishedSetup;
    [SerializeField] protected Vector3Channel RequestFocusLocationChannel;
    [SerializeField] protected CombatUnitChannel RequestStartFollowingUnitChannel;
    [SerializeField] protected Channel RequestStopFollowingUnitChannel;
    [SerializeField] protected Channel OnCombatStartChannel;
    [SerializeField] protected Channel OnCombatEndChannel;

    [SerializeField] private bool runSetupOnEnable = true;
    [SerializeField] private PlayerController controllerToConnectToOnEnable;
    [SerializeField] private PlayerCharacter playerCharacterToConnectToOnEnable;

    private bool canMoveCameraManually = true;
    private bool isFocusing = false;
    private bool isFollowingUnit = false;
    private Transform m_transformToFollow;

    protected UnityEvent OnReachFocusLocation;

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

        FindOrSpawnCamera();

        RequestFocusLocationChannel.channelEvent.AddListener(MoveCameraToLocation);
        RequestStartFollowingUnitChannel.channelEvent.AddListener(StartFollowingUnit);
        RequestStopFollowingUnitChannel.channelEvent.AddListener(StopFollowingUnit);
        OnCombatStartChannel.channelEvent.AddListener(OnCombatStartChan)

        OnReachFocusLocation = new UnityEvent();


        OnCameraManagerFinishedSetup.Raise(ID);
    }

    protected virtual void Teardown()
    {
        RequestFocusLocationChannel.channelEvent.RemoveListener(MoveCameraToLocation);
        RequestStartFollowingUnitChannel.channelEvent.RemoveListener(StartFollowingUnit);
        RequestStopFollowingUnitChannel.channelEvent.RemoveListener(StopFollowingUnit);
    }

    protected virtual void FindOrSpawnCamera()
    {
        camera = GameObject.FindFirstObjectByType<Camera>();

        if(camera == null)
        {
            GameObject obj = Instantiate(cameraPrefab);
            camera = obj.GetComponent<Camera>();
        }
    }


    public void MoveCameraManually(Vector2 direction)
    {
        if(canMoveCameraManually) camera.gameObject.GetComponent<Movement2D>().setDesiredMoveDirection(direction);
    }

    public void MoveCameraToLocation(Vector2 location)
    {
        StopAllCoroutines();
        
        canMoveCameraManually = true;
        isFocusing = false;
        MoveCameraManually(Vector2.zero);
        
        Vector3 loc = new Vector3(location.x, location.y, -10);

        StartCoroutine(FocusOnLocation(loc, camera.transform.position, 0.5f));
    }

    public void MoveCameraToLocation(Vector3 location)
    {
        MoveCameraToLocation(new Vector2(location.x, location.y));
    }

    public void StartFollowingUnit(CombatUnit unitToFollow)
    {
        // canMoveCameraManually = false;
        // isFollowingUnit = true;

        // m_transformToFollow = unitToFollow.transform;

        // OnReachFocusLocation.AddListener(StartFollowingLoop);
        // MoveCameraToLocation(m_transformToFollow.transform.position);

        StartFollowingTransform(unitToFollow.transform);
    }

    public void StartFollowingTransform(Transform transformToFollow)
    {
        canMoveCameraManually = false;
        isFollowingUnit = true;

        m_transformToFollow = transformToFollow;

        OnReachFocusLocation.AddListener(StartFollowingLoop);
        MoveCameraToLocation(m_transformToFollow.transform.position);
    }

    private void StartFollowingLoop()
    {
        StartCoroutine(FollowUnit());
    }

    public void StopFollowingUnit()
    {
        isFollowingUnit = false;
        OnReachFocusLocation.RemoveListener(StartFollowingLoop);
    }

    public void OnStartCombat()
    {

    }

    public void OnEndCombat()
    {

    }

    IEnumerator FocusOnLocation(Vector3 focusLocation, Vector3 startLocation, float timeToTake)
    {
        canMoveCameraManually = false;
        isFocusing = true;
        float currentTimer = 0f;
        while (currentTimer <= timeToTake)
        {
            currentTimer += Time.deltaTime;
            
            float i = currentTimer/timeToTake;
            Vector3 position = Vector3.Slerp(startLocation, focusLocation, i);
            camera.transform.position = position;

            yield return null;   
        }
        isFocusing = false;
        canMoveCameraManually = true;
        OnReachFocusLocation.Invoke();
    }

    IEnumerator FollowUnit()
    {
        while(isFollowingUnit)
        {
            // Debug.Log("Follow 1");
            
            Vector3 targetPosition = new Vector3(m_transformToFollow.position.x, m_transformToFollow.position.y, -10);
            
            Vector3 currentPosition = camera.transform.position;

            float currentTimer = 0f;
            float maxTime = 0.2f;

            while(currentTimer <= maxTime)
            {
                // Debug.Log("Follw 2");
                
                currentTimer += Time.deltaTime;

                float i = currentTimer/maxTime;

                Vector3 position = Vector3.Slerp(currentPosition, targetPosition, i);
                camera.transform.position = position;

                yield return null;
            }

            yield return null;
        }
    }
}
