using System.Collections.Generic;
using UnityEngine;

public class BombHandler : MonoBehaviour
{

    [Header("Stats")] 
    [SerializeField] 
    private float rotationSpeed = 5f;
    [SerializeField]
    private float rotSnapSpeed = 5f;

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
    [SerializeField]
    private Camera playerCamera;

    //Collection of all Minigames inclusive StartAndTimer at index 0
    [SerializeField] 
    private List<BombMinigame> minigames;
    
    
    [Header("Debug")]
    [SerializeField] 
    private bool debugUseKeyboard = false;

    [SerializeField] 
    private List<GameObject> bombDebugSides;

    [SerializeField] private Material debugMaterial;
    [SerializeField] private Material highlightDebugMaterial;
    
    
    [Header("Runtime Info")]
    [SerializeField]
    private bool canRotate = false;

    [SerializeField]
    private bool isInMinigame = false;

    [SerializeField] 
    private BombMinigame currentMinigame;
    [SerializeField]
    private Vector3 targetRotation = new Vector3(0f, 0f, 0f);
    
    [SerializeField]
    private Vector2 keyboardInput = new Vector2(0f, 0f);


    private bool isFirstStart = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnterMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate && debugUseKeyboard)
        {
            RotateBomb();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnterMinigame();
            }
        }

        if (isInMinigame)
        {
            LerpToTargetRotation();
        }
    }

    private void LerpToTargetRotation()
    {
        float xRot = Mathf.LerpAngle(
            cameraRig.transform.eulerAngles.x,
            targetRotation.x,
            rotSnapSpeed * Time.deltaTime
        );

        float yRot = Mathf.LerpAngle(
            cameraRig.transform.eulerAngles.y,
            targetRotation.y,
            rotSnapSpeed * Time.deltaTime
        );

        float zRot = Mathf.LerpAngle(
            cameraRig.transform.eulerAngles.z,
            targetRotation.z,
            rotSnapSpeed * Time.deltaTime
        );

        cameraRig.transform.eulerAngles = new Vector3(xRot, yRot, zRot);
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

        //Debug.Log($"[BombHandler] Rotation - X: {currentXRot}, Y: {currentYRot}");
        CheckDebugPlane();
    }

    private async void EnterMinigame()
    {
        DisableBombRotation();
        isInMinigame = true;
        //Wait till minigame is finished or failed
        bool isFinished = false;
        while (!isFinished)
        {
            if (isFirstStart)
            {
                isFinished = await minigames[0].EnterMinigame();
                isFirstStart = false;
            }
            else
            {
                //TODO: start minigame that is currently selected
                isFinished = await currentMinigame.EnterMinigame();
            }
            
        }

        isInMinigame = false;
        EnableBombRotation();
    }
    
    
    [SerializeField] private float rayDistance = 10f;
    private void CheckDebugPlane()
    {
        // 1. Create a Bitmask for the "Debug" layer (Layer 7 for example)
        // This tells the raycast to ONLY hit objects on this layer.
        int layerMask = LayerMask.GetMask("Debug");

        // 2. Define the Ray: Originating at the camera, pointing forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // 3. Perform the Raycast
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            foreach (GameObject plane in bombDebugSides)
            {
                plane.GetComponent<Renderer>().material = (hit.collider.gameObject == plane) ? highlightDebugMaterial : debugMaterial;
            }
            
            MinigameSide mgSide = hit.transform.gameObject.GetComponent<MinigameSide>();
            if (mgSide != null)
            {
                currentMinigame = mgSide.minigame;
                targetRotation = mgSide.targetRotation;
            }
        }
    
        // Optional: Visualize the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        
        
    }
}
