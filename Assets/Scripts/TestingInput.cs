using UnityEngine;

public class TestingInput : MonoBehaviour
{
    [SerializeField] string gameModeToLoadOnStart;
    [SerializeField] StringChannel onChangeGameModeChannel;
    [SerializeField] StringChannel changeSceneChannel;
    [SerializeField] Channel requestPauseOrResumeSongChannel;
    [SerializeField] Channel requestSkipSongChannel;
    [SerializeField] SongChannel requestQueueSongChannel;
    [SerializeField] SongData songToQueue;
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            onChangeGameModeChannel.Raise(gameModeToLoadOnStart);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            onChangeGameModeChannel.Raise("MainMenu");
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            onChangeGameModeChannel.Raise("GamePlay");
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeSceneChannel.Raise("MainMenu");
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeSceneChannel.Raise("GamePlay");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            requestPauseOrResumeSongChannel.Raise();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            requestSkipSongChannel.Raise();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            requestQueueSongChannel.Raise(songToQueue);
        }
    }
}