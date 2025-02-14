using UnityEngine;
using UnityEngine.Events;

public class SceneSetupManager : MonoBehaviour
{
    [SerializeField] private bool changeGameModeOnSceneStart = true;
    [SerializeField] private StringChannel onChangeGameModeChannel;
    [SerializeField] private string gameModeToChangeToOnSetup;
    [SerializeField] private bool changeSongOnSceneStart = true;
    [SerializeField] private SongChannel requestJumpToSongChannel;
    [SerializeField] private SongData songToJumpTo;
    [SerializeField] private SceneContext context;
    [SerializeField] private SceneContextChannel updateContextChannel;
    
    [SerializeField] bool triggerOnStart = true;
    public SceneContextEvent onSetup;

    public void Start()
    {
        if(triggerOnStart) Setup();
    }

    public void Setup()
    {
        if(updateContextChannel.lastValue.SpawnPointID != 0) context = updateContextChannel.lastValue;
        
        if(changeGameModeOnSceneStart) onChangeGameModeChannel.Raise(gameModeToChangeToOnSetup);
        if(changeSongOnSceneStart) requestJumpToSongChannel.Raise(songToJumpTo);
        onSetup.Invoke(context);

        // Add code here
    }
}
