using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] List<StringAndGameObject> allGameModes;
    [SerializeField] StringAndGameObject nextGameModeToLoad;
    
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] AnimationCurve playerPrefAudioValueToAppliedValue;

    private GameMode activeGameMode;

    // [Header: "Channels"]
    [SerializeField] Channel onOptionsChangedChannel;
    [SerializeField] StringChannel requestChangeGameModeChannel;
    [SerializeField] Channel onGameModeTeardownFinishedChannel;
    [SerializeField] Channel onGameModeSetupFinishedChannel;
    [SerializeField] Channel requestGameModeTearDownChannel;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string FullScreenKey = "FullScreen";
    private const string ResolutionKey = "Resolution";

    // CHANELS =================================

    private void Start()
    {
        // According to Unity Docs making calls to the Audio Mixer in OnEnable will not work
        // So this function will be run in Start() instead
        SetupOptions();
    }
    
    private void OnEnable()
    {
        SetupChannels();
    }

    private void OnDisable()
    {
        TearDownChannels();
    }

    private void SetupChannels()
    {
        onOptionsChangedChannel.channelEvent.AddListener(SetupOptions);
        requestChangeGameModeChannel.channelEvent.AddListener(RequestChangeGameMode);
        onGameModeTeardownFinishedChannel.channelEvent.AddListener(OnGameModeTeardownFinished);
        onGameModeSetupFinishedChannel.channelEvent.AddListener(OnGameModeSetupFinished);
    }

    private void TearDownChannels()
    {
        onOptionsChangedChannel.channelEvent.RemoveListener(SetupOptions);
        requestChangeGameModeChannel.channelEvent.RemoveListener(RequestChangeGameMode);
        onGameModeTeardownFinishedChannel.channelEvent.RemoveListener(OnGameModeTeardownFinished);
        onGameModeSetupFinishedChannel.channelEvent.RemoveListener(OnGameModeSetupFinished);
    }

    // CHANNEL RESPONES =================================

    private void RequestChangeGameMode(string newGameMode)
    {
        // Checks that we are not loading the active gamemode
        if(newGameMode == nextGameModeToLoad.name)
        {
            Debug.LogWarning("Something just tried to change the gamemode to the currently active gamemode");
            return;
        }

        // Checks the requested gamemode is valide
        if(allGameModes.Exists(x => x.name == newGameMode))
        {
            nextGameModeToLoad = allGameModes.Find(x => x.name == newGameMode);


            // Checks if there is an active gamemode
            if(activeGameMode != null)
            {
                // Tearsdown the currently active GameMode, once that is done it GameManager will setup the new GameMode
                requestGameModeTearDownChannel.Raise();
                return;
            }
            else
            {
                // Jumps straight to loading the new gamemode
                OnGameModeTeardownFinished();
            }
            
        }
        else
        {
            Debug.LogWarning("Something tried to load the gamemode " + newGameMode + " which does not exist on the current GameManager.  Double check your spelling");
        }
    }

    private void OnGameModeTeardownFinished()
    {
        // Passing in nextGameModeToLoad as a prefab but it becomes a scene object
        LoadGameMode(nextGameModeToLoad.obj);
        SetupGameMode(activeGameMode);
    } 

    private void OnGameModeSetupFinished()
    {}

    // MAIN FUNCTIONS =================================

    public void SetupOptions()
    {
        Debug.Log("GameManager: SetupOptions()");
        
        float masterVolumeStoredValue = PlayerPrefs.GetFloat(MasterVolumeKey, 1.0f);
        float musicVolumeStoredValue = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        float sfxVolumeStoredValue = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
        int fullScreen = PlayerPrefs.GetInt(FullScreenKey, 0);
        int resolution = PlayerPrefs.GetInt(ResolutionKey, 0);

        bool fullScreenValue;

        if(fullScreen == 1) fullScreenValue = true;
        else fullScreenValue = false;


        // float masterVolumeAppliedValue = masterVolumeStoredValue.Remap(0f,1f,-80f,0f);
        // float musicVolumeAppliedValue = musicVolumeStoredValue.Remap(0f,1f,-80f,0f);
        // float sfxVolumeAppliedValue = sfxVolumeStoredValue.Remap(0f,1f,-80f,0f);

        float masterVolumeAppliedValue = playerPrefAudioValueToAppliedValue.Evaluate(masterVolumeStoredValue);
        float musicVolumeAppliedValue = playerPrefAudioValueToAppliedValue.Evaluate(musicVolumeStoredValue);
        float sfxVolumeAppliedValue = playerPrefAudioValueToAppliedValue.Evaluate(sfxVolumeStoredValue);


        masterMixer.SetFloat(MasterVolumeKey, masterVolumeAppliedValue);
        masterMixer.SetFloat(MusicVolumeKey, musicVolumeAppliedValue);
        masterMixer.SetFloat(SFXVolumeKey, sfxVolumeAppliedValue);

        Screen.fullScreen = fullScreenValue;

        int width = 0;
        int height = 0;

        switch (resolution)
        {
            case 0:
                width = 1920;
                height = 1080;
                break;
            case 1:
                width = 1280;
                height = 720;
                break;
            case 2:
                width = 720;
                height = 480;
                break;
            default:
                break;
        }

        Screen.SetResolution(width, height, fullScreenValue);

        Debug.Log("masterVolume StoredValue: " + masterVolumeStoredValue + "\nmasterVolume Applied: " + masterVolumeAppliedValue);
        Debug.Log("musicVolume StoredValue: " + musicVolumeStoredValue + "\nmusicVolume Applied: " + musicVolumeAppliedValue);
        Debug.Log("sfxVolume StoredValue: " + sfxVolumeStoredValue + "\nsfxVolume Applied: " + sfxVolumeAppliedValue);
        Debug.Log("fullscreen value: " + fullScreenValue);
        Debug.Log("resolution stored: " + resolution + "\nApplied: " + width + " x " + height);
    }

    public void LoadGameMode(GameObject newGameMode)
    {
        nextGameModeToLoad.obj = Instantiate(newGameMode);
        activeGameMode = nextGameModeToLoad.obj.GetComponent<GameMode>();
    }

    public void SetupGameMode(GameMode gameModeToSetup)
    {
        gameModeToSetup.Setup();
    }

}
