using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] protected int ID;
    [SerializeField] protected Character possessedCharacter;
    

    [SerializeField] protected IntChannel OnControllerFinishedSetup;

    [SerializeField] private bool runSetupOnEnable = true;
    [SerializeField] private Character characterToPossessOnEnable;


    // POSSESSION ===========================================================
    public virtual bool PossessCharacter(Character newCharacter)
    {
        if(!IsCharacterValid(newCharacter)) return false;
        
        if(newCharacter == possessedCharacter) return true;

        if(newCharacter.Possess(this))
        {
            possessedCharacter = newCharacter;
            possessedCharacter.SetID(ID);
            return true;
        }

        Debug.LogWarning("The Controller: " + this + " tried to possess the character: " + newCharacter + " but it is already being possessed.  If this is intential use ForcePossessCharacter instead");
        return false;
    }

    public virtual void ForcePossessCharacter(Character newCharacter)
    {
        if(!IsCharacterValid(newCharacter)) return;
        if(newCharacter == possessedCharacter) return;

        newCharacter.ForcePossess(this);
        possessedCharacter = newCharacter;
        possessedCharacter.SetID(ID);
    }

    public void UnPossessCharacter()
    {
        if(IsCharacterValid(possessedCharacter)) possessedCharacter.UnPossess();
        possessedCharacter = null;
    }

    public void SetID(int newID)
    {
        if(ID == newID) return;
        
        ID = newID;
        if(IsCharacterValid(possessedCharacter)) possessedCharacter.SetID(ID);
    }

    protected virtual bool IsCharacterValid(Character newCharacter)
    {
        if(newCharacter != null) return true;
        else return false;
    }

    // SETUP AND TEARDOWN ===========================================================

    protected virtual void OnEnable()
    {
        if(runSetupOnEnable) Setup(ID, characterToPossessOnEnable);
    }

    protected virtual void OnDisable()
    {
        Teardown();
    }

    public virtual void Setup(int newID = 1, Character characterToPossess = null)
    {
        ID = newID;

        if(PossessCharacter(characterToPossess))
        {
            OnControllerFinishedSetup.Raise(ID);
        }
    }

    public virtual void Teardown()
    {

    }

}
