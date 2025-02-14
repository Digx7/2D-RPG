using UnityEngine;

public class GameMode : MonoBehaviour
{
    public bool spawnPlayerOnSetup = true;
    public GameObject playerCharacterPreFab;
    public GameObject playerControllerPreFab;
    public GameObject cameraManagerPreFab;
    
    [SerializeField] private Channel requestGameModeTearDownChannel;
    [SerializeField] private Channel onGameModeTearDownFinsishedChannel;
    [SerializeField] private Channel onGameModeSetupFinishedChannel;
    [SerializeField] private PlayerSpawnInfoChannel requestSpawnPlayerChannel;
    [SerializeField] private IntChannel onPlayerCharacterFinishedSetupChannel;
    [SerializeField] private IntChannel onPlayerControllerFinishedSetupChannel;
    [SerializeField] private Channel onOptionsMenuQuitChannel;

    private PlayerSpawnInfo playerSpawnInfo;
    private PlayerCharacter playerCharacterBeingSetup;
    private PlayerController playerControllerBeingSetup;
    private CameraManager cameraManagerBeingSetup;

    // CHANNELS =================================

    protected virtual void SetupChannels()
    {
        requestGameModeTearDownChannel.channelEvent.AddListener(Teardown);
        requestSpawnPlayerChannel.channelEvent.AddListener(SpawnPlayerCharacter);
        onPlayerCharacterFinishedSetupChannel.channelEvent.AddListener(OnPlayerCharacterFinishedSetup);
        onPlayerControllerFinishedSetupChannel.channelEvent.AddListener(OnPlayerControllerFinishedSetup);
        onOptionsMenuQuitChannel.channelEvent.AddListener(OnOptionsMenuQuit);
    }

    protected virtual void TearDownChannels()
    {
        requestGameModeTearDownChannel.channelEvent.RemoveListener(Teardown);
        requestSpawnPlayerChannel.channelEvent.RemoveListener(SpawnPlayerCharacter);
        onPlayerCharacterFinishedSetupChannel.channelEvent.RemoveListener(OnPlayerCharacterFinishedSetup);
        onPlayerControllerFinishedSetupChannel.channelEvent.RemoveListener(OnPlayerControllerFinishedSetup);
        onOptionsMenuQuitChannel.channelEvent.RemoveListener(OnOptionsMenuQuit);
    }

    // MAIN FUNCTIONS =================================

    public virtual void Setup()
    {
        SetupChannels();
        if(spawnPlayerOnSetup) TryToFindPlayerSpawnHelper();
        
        DontDestroyOnLoad(this);
        onGameModeSetupFinishedChannel.Raise();
    }

    public virtual void Teardown()
    {
        TearDownChannels();
        onGameModeTearDownFinsishedChannel.Raise();
        Destroy(this.gameObject);
    }

    // SPAWN PLAYER ================================================

    protected virtual void TryToFindPlayerSpawnHelper()
    {
        // We are making us of the SceneSetupManagers
        // PlayerSpawnHelper playerSpawnHelper = FindFirstObjectByType<PlayerSpawnHelper>();
        // if(playerSpawnHelper != null) playerSpawnHelper.SpawnPlayer();
    }

    protected virtual void SpawnPlayerCharacter(PlayerSpawnInfo newPlayerSpawnInfo)
    {
        Debug.Log("GameMode: SpawnPlayerCharacter()");

        playerSpawnInfo = newPlayerSpawnInfo;
        
        GameObject characterObj = Instantiate(playerCharacterPreFab, playerSpawnInfo.location, playerSpawnInfo.rotation);
        playerCharacterBeingSetup = characterObj.GetComponent<PlayerCharacter>();
        if(playerCharacterBeingSetup == null) return;

        playerCharacterBeingSetup.Setup(playerSpawnInfo.ID);
    }

    protected virtual void SpawnPlayerController()
    {
        Debug.Log("GameMode: SpawnPlayerController()");
        
        GameObject controllerObj = Instantiate(playerControllerPreFab, playerSpawnInfo.location, playerSpawnInfo.rotation);
        playerControllerBeingSetup = controllerObj.GetComponent<PlayerController>();
        if(playerControllerBeingSetup == null) return;

        playerControllerBeingSetup.Setup(playerSpawnInfo.ID, playerCharacterBeingSetup);
    }

    protected virtual void SpawnCameraManager()
    {
        Debug.Log("GameMode: SpawnCameraManager()");
        
        GameObject cameraObj = Instantiate(cameraManagerPreFab, playerSpawnInfo.location, playerSpawnInfo.rotation);
        cameraManagerBeingSetup = cameraObj.GetComponent<CameraManager>();
        if(cameraManagerBeingSetup == null) return;

        cameraManagerBeingSetup.Setup(playerSpawnInfo.ID, playerControllerBeingSetup, playerCharacterBeingSetup);
    }

    protected virtual void OnPlayerCharacterFinishedSetup(int playerId)
    {
        SpawnPlayerController();
    }

    protected virtual void OnPlayerControllerFinishedSetup(int playerId)
    {
        SpawnCameraManager();
    }

    protected virtual void OnOptionsMenuQuit()
    {}

}
