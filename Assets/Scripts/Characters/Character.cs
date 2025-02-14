using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int ID = 0;
    [SerializeField] protected GameController controller;

    protected virtual void Awake() {}

    protected virtual void Start() {}

    protected virtual void Update() {}
    
    // POSSESSION ===========================================================

    public bool Possess(GameController newController)
    {
        if(!IsControllerValid(newController)) return false;

        if(newController == controller) return true;
        
        if (controller != null)
        {
            Debug.LogWarning("A controller tried to possess this character, but it is already possessed by: " + controller);
            return false;
        }

        controller = newController;
        return true;
    }

    public void ForcePossess(GameController newController)
    {
        if(!IsControllerValid(newController)) return;
        if(newController == controller) return;
        
        if (controller != null)
        {
            controller.UnPossessCharacter();
        }
        controller = newController;
    }

    public void SetID(int newID)
    {
        if(ID == newID) return;
        
        ID = newID;
        if(controller != null) controller.SetID(ID);
    }

    public void UnPossess()
    {
        ID = 0;
        controller = null;
    }

    private bool IsControllerValid(GameController newController)
    {
        if(newController == null) return false;
        else return true;
    }

    // SETUP AND TEARDOWN ===================================================================

    protected virtual void OnEnable()
    {
        Setup();
    }

    protected virtual void OnDisable()
    {
        Teardown();
    }

    public virtual void Setup(int newID = 0)
    {
        SetID(newID);
    }

    public virtual void Teardown()
    {

    }

}
