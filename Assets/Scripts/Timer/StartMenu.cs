using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : BombMinigame
{
    [Header("Components")] 
    [SerializeField]
    private BombTimer bombTimer;

    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button exitButton;
    
    private Button currentButton = null;

    private float inputCooldown = 0.2f;
    private float lastInputTime;
    
    
    public override void SetEventListeners()
    {
        IOHandler.joystickUsed.AddListener(NavigateUI);
        IOHandler.buttonUsed.AddListener(SelectButton);
    }
    
    public override void UnbindEventListeners()
    {
        IOHandler.joystickUsed.RemoveListener(NavigateUI);
        IOHandler.buttonUsed.RemoveListener(SelectButton);
    }
    

    public override void StartMinigame()
    {
        
    }

    public override async Awaitable<bool> EnterMinigame()
    {
        SetEventListeners();
        SetButton(ref startButton);
        SwitchButtons(true);

        while (!wantsToExit)
        {
            await Awaitable.NextFrameAsync();
        }
        return true;
    }

    private void SwitchButtons(bool b)
    {
        pauseButton.interactable = b;
        exitButton.interactable = b;
    }

    public override void ExitMinigame()
    {
        UnbindEventListeners();
        SwitchButtons(false);
        EventSystem.current.SetSelectedGameObject(null);
        wantsToExit = true;
    }
    
    
    //Process Joystick Inputs and switch between selected buttons
    private void NavigateUI()
    {
        Vector2 input = IOHandler.joystickInput;
        Debug.Log("Navigate UI");
        // prevent spamming
        if (Time.time - lastInputTime < inputCooldown) return;

        // ignore tiny input
        if (input.magnitude < 0.5f) return;

        // normalize direction
        input.Normalize();

        if (currentButton == startButton)
        {
            if (input.y > 0.25f) // right → Pause
            {
                SetButton(ref pauseButton);
            }
            else if (input.x < -0.25f) // down → Exit
            {
                SetButton(ref exitButton);
            }
        }
        else if (currentButton == pauseButton)
        {
            if (input.y < -0.25f) // left → Start
            {
                SetButton(ref startButton);
            }
            else if (input.x < -0.25f) // down → Exit
            {
                SetButton(ref exitButton);
            }
        }
        else if (currentButton == exitButton)
        {
            if (input.x > 0.25f) // up → decide left/right
            {
                if (input.y < 0)
                    SetButton(ref startButton);
                else
                    SetButton(ref pauseButton);
            }
        }

        lastInputTime = Time.time;
    }
    
    //Perform Action associated with button
    private void SelectButton()
    {
        currentButton.onClick.Invoke();
    }

    private void SetButton(ref Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        currentButton = button;
    }

    public void OnStartPressed()
    {
        startButton.interactable = false; //Disable start button since the game is already started
        bombTimer.StartTimer();
        ExitMinigame();
    }

    public void OnPausePressed()
    {
        bombTimer.PauseResumeTimer();
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}