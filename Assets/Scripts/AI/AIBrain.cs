using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] protected int ID;
    [SerializeField] protected AIController aIController;

    [SerializeField] private IntChannel onAIBrainFinishedSetupChannel;

    public void SetID(int newID)
    {
        if(newID == ID) return;

        ID = newID;
        if(aIController != null)aIController.SetID(newID);
    }

    public bool ConnectAIController(AIController newAIController)
    {
        if(!IsAIControllerValid(newAIController)) return false;

        if(newAIController == aIController) return true;
        
        if(aIController != null)
        {
            Debug.LogWarning("The AIBrain: " + newAIController + " tried to connect to the AIController " + this + " but it is already connected to the AIBrain: " + aIController + ".  If this was intentional use ForceConnectAIBrain instead");
            return false;
        }

        aIController = newAIController;
        aIController.SetID(ID);
        return true;
    }

    public void ForceConnectAIController(AIController newAIController)
    {
        if(!IsAIControllerValid(newAIController))return;
        if(newAIController == aIController)return;

        aIController = newAIController;
        aIController.SetID(ID);
    }

    private bool IsAIControllerValid(AIController newAIController)
    {
        if(newAIController == null) return false;
        else return true;
    }

    protected virtual void OnDisable()
    {
        Teardown();
    }

    public void Setup(int newID, AIController newAIController = null)
    {
        ConnectAIController(newAIController);
        SetID(newID);
        onAIBrainFinishedSetupChannel.Raise(ID);
    }

    protected virtual void Teardown()
    {

    }
}
