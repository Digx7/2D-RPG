using UnityEngine;

public class PlayerSpawnHelper : MonoBehaviour
{
    [SerializeField] private PlayerSpawnInfoChannel requestSpawnPlayerChannel;
    
    public void SpawnPlayer()
    {
        Debug.Log("PlayerSpawnHelper:  SpawnPlayer()");
        
        PlayerSpawnInfo playerSpawnInfo = new PlayerSpawnInfo();

        playerSpawnInfo.ID = 1;
        playerSpawnInfo.location = this.transform.position;
        playerSpawnInfo.rotation = this.transform.rotation;

        requestSpawnPlayerChannel.Raise(playerSpawnInfo);
    }
}
