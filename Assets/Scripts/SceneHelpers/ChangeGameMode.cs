using UnityEngine;
using UnityEngine.Events;

public class ChangeGameMode : MonoBehaviour
{
    [SerializeField] private StringChannel requestChangeGameModeChannel;
    [SerializeField] private string gameModeToChangeTo;

    public void Change()
    {
        requestChangeGameModeChannel.Raise(gameModeToChangeTo);
    }
}
