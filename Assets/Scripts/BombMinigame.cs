using UnityEngine;

public abstract class BombMinigame : MonoBehaviour
{
    public bool isStarted = false;

    
    //(Un)Bind all needed physical IODevices with those two functions 
    //Use IOHadler.xyzUsed.AddListener(FUNCTION_NAME)
    public abstract void SetEventListeners();
    //Use IOHadler.xyzUsed.RemoveListener(FUNCTION_NAME)
    public abstract void UnbindEventListeners();
    
    //Starts the minigame, regardless of whether the player is here or not
    public abstract void StartMinigame();
    
    //Enter the minigame --> bind listeners, etc.
    //Should enable the minigame so the player can try to fix it
    public abstract void EnterMinigame();
    
    //When player exits the minigame while it's active or finishes it --> unbind listeners
    //just exits the minigame
    public abstract void ExitMinigame();
}
