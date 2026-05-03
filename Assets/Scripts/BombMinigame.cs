using UnityEngine;

public abstract class BombMinigame : MonoBehaviour
{
    public bool isStarted = false;


    public abstract void SetEventListeners();
    public abstract void UnbindEventListeners();
    
    //Starts the minigame, regardless of whether the player is here or not
    public abstract void StartMinigame();
    
    //Enter the minigame --> bind listeners, etc.
    public abstract void EnterMinigame();
    
    //When player exits the minigame while it's active or finishes it --> unbind listeners
    public abstract void ExitMinigame();
}
