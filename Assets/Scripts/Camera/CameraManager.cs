using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] protected int ID = 1;
    [HideInInspector][SerializeField] public Camera camera;
    [HideInInspector][SerializeField] public GameObject cameraPrefab;
    [HideInInspector][SerializeField] PlayerController connectedPlayerController;
    [HideInInspector][SerializeField] PlayerCharacter playerCharacter;

    [SerializeField] protected IntChannel OnCameraManagerFinishedSetup;
    [SerializeField] protected Vector3Channel RequestFocusLocationChannel;
    [SerializeField] protected CombatUnitChannel RequestStartFollowingUnitChannel;
    [SerializeField] protected Channel RequestStopFollowingUnitChannel;
    [SerializeField] protected Channel OnCombatStartChannel;
    [SerializeField] protected Channel OnCombatEndChannel;
    [SerializeField] protected SceneContextChannel contextOnSceneSetupChannel;

    [SerializeField] private bool runSetupOnEnable = true;
    [SerializeField] private PlayerController controllerToConnectToOnEnable;
    [SerializeField] private PlayerCharacter playerCharacterToConnectToOnEnable;

    private bool canMoveCameraManually = true;
    private bool isFocusing = false;
    private bool isFollowingUnit = false;
    private Transform m_transformToFollow;
    private Vector2 m_followOffset;
    private SceneCameraMode m_sceneCameraMode = SceneCameraMode.FollowPlayer;

    private static readonly Vector2 PLAYER_FOLLOW_OFFSET = new Vector2(0, 5);
    private float m_xFollowDamping = 20;
    private float m_yFollowDamping = 20;

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
            Debug.Log(playerCharacter);
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
        m_followOffset = new Vector2();
        if (runSetupOnEnable) Setup(ID, controllerToConnectToOnEnable, playerCharacterToConnectToOnEnable);
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
        RequestStopFollowingUnitChannel.channelEvent.AddListener(StopFollowing);
        OnCombatStartChannel.channelEvent.AddListener(OnStartCombat);
        OnCombatEndChannel.channelEvent.AddListener(OnEndCombat);
        contextOnSceneSetupChannel.channelEvent.AddListener(OnSceneChange);

        OnReachFocusLocation = new UnityEvent();


        // if (contextOnSceneSetupChannel.lastValue.sceneCameraMode == SceneCameraMode.FollowPlayer)
        // {
        //     WarpCameraToTransform(playerCharacter.transform);
        //     StartFollowingTransform(playerCharacter.transform);
        // }

        OnSceneChange(contextOnSceneSetupChannel.lastValue);



        OnCameraManagerFinishedSetup.Raise(ID);
    }

    protected virtual void Teardown()
    {
        RequestFocusLocationChannel.channelEvent.RemoveListener(MoveCameraToLocation);
        RequestStartFollowingUnitChannel.channelEvent.RemoveListener(StartFollowingUnit);
        RequestStopFollowingUnitChannel.channelEvent.RemoveListener(StopFollowing);
        OnCombatStartChannel.channelEvent.RemoveListener(OnStartCombat);
        OnCombatEndChannel.channelEvent.RemoveListener(OnEndCombat);
        contextOnSceneSetupChannel.channelEvent.RemoveListener(OnSceneChange);
    }

    // MAIN FOLLOW FUNCTIONS =================================================================

    public virtual void OnSceneChange(SceneContext newSceneContext)
    {
        m_sceneCameraMode = newSceneContext.sceneCameraMode;

        if (m_sceneCameraMode == SceneCameraMode.FollowPlayer)
        {
            Vector3 playerLoc = new Vector3(
                playerCharacter.transform.position.x + PLAYER_FOLLOW_OFFSET.x,
                playerCharacter.transform.position.y + PLAYER_FOLLOW_OFFSET.y,
                playerCharacter.transform.position.z
            );

            // WarpCameraToTransform(playerCharacter.transform);
            WarpCameraToLocation(playerLoc);
            StartFollowingTransform(playerCharacter.transform, PLAYER_FOLLOW_OFFSET, new Vector2(20,5));
        }
        else if (m_sceneCameraMode == SceneCameraMode.Static)
        {
            WarpCameraToLocation(newSceneContext.cameraLocation);
        }
    }

    protected virtual void FindOrSpawnCamera()
    {
        camera = GameObject.FindFirstObjectByType<Camera>();

        if (camera == null)
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
        StartFollowingTransform(unitToFollow.transform);
    }

    public void StartFollowingUnit(CombatUnit unitToFollow, Vector2 offset)
    {
        StartFollowingTransform(unitToFollow.transform, offset);
    }

    public void StartFollowingUnit(CombatUnit unitToFollow, Vector2 offset, Vector2 damping)
    {
        StartFollowingTransform(unitToFollow.transform, offset, damping);
    }

    public void StartFollowingTransform(Transform transformToFollow)
    {
        m_followOffset = Vector2.zero;
        m_xFollowDamping = 20;
        m_yFollowDamping = 20;
        canMoveCameraManually = false;
        isFollowingUnit = true;

        m_transformToFollow = transformToFollow;

        Vector3 focusLoc = new Vector3(
            m_transformToFollow.transform.position.x + m_followOffset.x,
            m_transformToFollow.transform.position.y + m_followOffset.y,
            m_transformToFollow.transform.position.z
        );

        OnReachFocusLocation.AddListener(StartFollowingLoop);
        MoveCameraToLocation(focusLoc);
    }

    public void StartFollowingTransform(Transform transformToFollow, Vector2 offset)
    {
        m_followOffset = offset;
        m_xFollowDamping = 20;
        m_yFollowDamping = 20;
        canMoveCameraManually = false;
        isFollowingUnit = true;

        m_transformToFollow = transformToFollow;

        Vector3 focusLoc = new Vector3(
            m_transformToFollow.transform.position.x + m_followOffset.x,
            m_transformToFollow.transform.position.y + m_followOffset.y,
            m_transformToFollow.transform.position.z
        );

        OnReachFocusLocation.AddListener(StartFollowingLoop);
        MoveCameraToLocation(focusLoc);
    }

    public void StartFollowingTransform(Transform transformToFollow, Vector2 offset, Vector2 damping)
    {
        m_followOffset = offset;
        m_xFollowDamping = damping.x;
        m_yFollowDamping = damping.y;
        canMoveCameraManually = false;
        isFollowingUnit = true;

        m_transformToFollow = transformToFollow;

        Vector3 focusLoc = new Vector3(
            m_transformToFollow.transform.position.x + m_followOffset.x,
            m_transformToFollow.transform.position.y + m_followOffset.y,
            m_transformToFollow.transform.position.z
        );

        OnReachFocusLocation.AddListener(StartFollowingLoop);
        MoveCameraToLocation(focusLoc);
    }

    public void WarpCameraToTransform(Transform transformToWarpTo)
    {
        WarpCameraToLocation(new Vector2(transformToWarpTo.position.x, transformToWarpTo.position.y));
    }

    public void WarpCameraToLocation(Vector3 location)
    {
        WarpCameraToLocation(new Vector2(location.x, location.y));
    }

    public void WarpCameraToLocation(Vector2 location)
    {
        canMoveCameraManually = false;
        isFollowingUnit = false;

        StopAllCoroutines();

        Vector3 target = new Vector3(location.x, location.y, -10);
        camera.transform.position = target;
    }

    private void StartFollowingLoop()
    {
        StartCoroutine(FollowUnit());
    }

    public void StopFollowing()
    {
        isFollowingUnit = false;
        OnReachFocusLocation.RemoveListener(StartFollowingLoop);
    }

    public void OnStartCombat()
    {
        StopFollowing();
    }

    public void OnEndCombat()
    {
        StartFollowingTransform(playerCharacter.transform, PLAYER_FOLLOW_OFFSET, new Vector2(20, 5));
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
            
            Vector3 targetPosition = new Vector3(m_transformToFollow.position.x + m_followOffset.x, m_transformToFollow.position.y + m_followOffset.y, -10);
            
            Vector3 currentPosition = camera.transform.position;

            // Vector3 smoothedPosition = Vector3.Lerp(currentPosition, targetPosition, 20 * Time.deltaTime);

            float smoothedXPos = Mathf.Lerp(currentPosition.x, targetPosition.x, m_xFollowDamping * Time.deltaTime);
            float smoothedYPos = Mathf.Lerp(currentPosition.y, targetPosition.y, m_yFollowDamping * Time.deltaTime);

            Vector3 smoothedPosition = new Vector3(smoothedXPos, smoothedYPos, -10);

            camera.transform.position = smoothedPosition;

            yield return null;
        }
    }
}
