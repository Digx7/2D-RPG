using UnityEngine;

public class AIController : GameController
{
    [SerializeField] protected AIBrain aiBrain;
    private AICharacter possessedAI;

    // OVERRIDE FUNCTIONS ==============================================

    public override bool PossessCharacter(Character newCharacter)
    {
        possessedAI = newCharacter as AICharacter;

        return base.PossessCharacter(newCharacter);
    }

    public override void ForcePossessCharacter(Character newCharacter)
    {
        possessedAI = newCharacter as AICharacter;

        base.ForcePossessCharacter(newCharacter);
    }

    // AI BRAIN FUNCTIONS ===================================================

    public bool ConnectAIBrain(AIBrain newAIBrain)
    {
        if(!IsAIBrainValid(newAIBrain)) return false;

        if(newAIBrain == aiBrain) return true;
        
        if(aiBrain != null)
        {
            Debug.LogWarning("The AIBrain: " + newAIBrain + " tried to connect to the AIController " + this + " but it is already connected to the AIBrain: " + aiBrain + ".  If this was intentional use ForceConnectAIBrain instead");
            return false;
        }

        aiBrain = newAIBrain;
        aiBrain.SetID(ID);
        return true;
    }

    public void ForceConnectAIBrain(AIBrain newAIBrain)
    {
        if(!IsAIBrainValid(newAIBrain))return;
        if(newAIBrain == aiBrain)return;

        aiBrain = newAIBrain;
        aiBrain.SetID(ID);
    }

    private bool IsAIBrainValid(AIBrain newAIBrain)
    {
        if(newAIBrain == null) return false;
        else return true;
    }

}
