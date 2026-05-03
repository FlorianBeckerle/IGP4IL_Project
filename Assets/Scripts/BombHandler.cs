using System.Collections.Generic;
using UnityEngine;

public class BombHandler : MonoBehaviour
{

    [Header("Stats")] 
    [SerializeField] 
    private float rotationSpeed = 5f;

    [SerializeField] 
    private float maxXRot = 20f;
    private float currentXRot = 0f;
    private float currentYRot = 0f;
    
    [SerializeField] 
    private bool inverseRotationX = false;
    
    [SerializeField] 
    private bool inverseRotationY = false;
    
    
    [Header("Components")]
    [SerializeField] 
    private GameObject cameraRig;

    //Collection of all Minigames 
    [SerializeField] 
    private List<BombMinigame> minigames;
    
    
    [Header("Debug")]
    [SerializeField] 
    private bool debugUseKeyboard = false;
    
    
    [Header("Runtime Info")]
    [SerializeField]
    private bool canRotate = false;
    [SerializeField]
    private Vector2 keyboardInput = new Vector2(0f, 0f);
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnableBombRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate && debugUseKeyboard)
        {
            RotateBomb();
        }
    }

    private Vector2 HandleKeyboardInput()
    {
        Vector2 input = Vector2.zero;

        // Handle Vertical (X in your request, usually Y in Unity)
        if (Input.GetKey(KeyCode.W)) input.x += 1f;
        if (Input.GetKey(KeyCode.S)) input.x -= 1f;

        // Handle Horizontal (Y in your request, usually X in Unity)
        if (Input.GetKey(KeyCode.D)) input.y += 1f;
        if (Input.GetKey(KeyCode.A)) input.y -= 1f;

        // Optional: Normalize to prevent faster diagonal movement
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        return input;
    }

    //When exiting a Minigame
    public void EnableBombRotation()
    {
        SetEventListeners();
        canRotate = true;
    }

    //When entering a Minigame
    public void DisableBombRotation()
    {
        UnbindEventListeners();
        canRotate = false;
    }

    public void SetEventListeners()
    {
        IOHandler.joystickUsed.AddListener(RotateBomb);
        IOHandler.buttonUsed.AddListener(EnterMinigame);
    }

    public void UnbindEventListeners()
    {
        IOHandler.joystickUsed.RemoveListener(RotateBomb);
        IOHandler.buttonUsed.RemoveListener(EnterMinigame);
        
    }

    private void RotateBomb()
    {
        Vector2 input = debugUseKeyboard ? HandleKeyboardInput() : IOHandler.joystickInput;
    
        // Applying sensitivity and frame-rate independence
        input *= rotationSpeed * Time.deltaTime;

        if (inverseRotationX) input.x *= -1f;
        if (inverseRotationY) input.y *= -1f;

        // 1. Vertical Rotation (X-axis) - Clamped
        currentXRot += input.x; 
        currentXRot = Mathf.Clamp(currentXRot, -maxXRot, maxXRot);

        // 2. Horizontal Rotation (Y-axis) - Unclamped
        currentYRot += input.y; 

        // 3. Apply both to the localRotation
        cameraRig.transform.localRotation = Quaternion.Euler(currentXRot, currentYRot, 0f);

        Debug.Log($"[BombHandler] Rotation - X: {currentXRot}, Y: {currentYRot}");
    }

    private void EnterMinigame()
    {
        DisableBombRotation();
        //Wait till minigame is finished or failed
        //TODO: maybe make async? 
        //EnableBombRotation();
    }
    
    
}
