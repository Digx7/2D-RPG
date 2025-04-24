using UnityEngine;

public class PlayerSpawnHelper : MonoBehaviour
{
    [SerializeField] private int ID = 0;
    [SerializeField] private PlayerSpawnInfoChannel requestSpawnPlayerChannel;
    [SerializeField] private SceneContextChannel contextOnSceneSetupChannel;


    private void OnEnable()
    {
        contextOnSceneSetupChannel.channelEvent.AddListener(SpawnPlayer);
    }

    private void OnDisable()
    {
        contextOnSceneSetupChannel.channelEvent.RemoveListener(SpawnPlayer);
    }

    public void SpawnPlayer(SceneContext context)
    {
        if(context.SpawnPointID == ID)
        {
            Debug.Log("PlayerSpawnHelper:  SpawnPlayer()");
        
            PlayerSpawnInfo playerSpawnInfo = new PlayerSpawnInfo();

            playerSpawnInfo.ID = 1;
            playerSpawnInfo.location = this.transform.position;
            playerSpawnInfo.rotation = this.transform.rotation;

            requestSpawnPlayerChannel.Raise(playerSpawnInfo);
        }
    }
}
