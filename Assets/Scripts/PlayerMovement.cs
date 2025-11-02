using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(CharacterController))]
// public class PlayerMovement : MonoBehaviour
// {
//     public float walkSpeed = 2f;
//     public float gravity = -9.81f;
//     // Added for if we want to manually modify the player's mouse sens
//     public float mouseSensitivity = 100f;

//     public float bobFrequency = 12f;
//     public float bobAmplitude = 0.1f;

//     public Transform cameraTransform;
//     private bool lookInputDisabled = true; 

//     private CharacterController controller;
//     private InputSystem_Actions actions;
//     private float verticalVelocity;
//     private float xRotation;

//     private float bobTimer;
//     private Vector3 cameraStartPos;

//     // Input values
//     private Vector2 moveInput;
//     private Vector2 lookInput;

//     void Awake()
//     {
//         controller = GetComponent<CharacterController>();
//         actions = new InputSystem_Actions();

//         // Move input
//         actions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//         actions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

//         // Look input
//         actions.Player.Look.performed += ctx => {
//             lookInput = ctx.ReadValue<Vector2>();
//         };
//         actions.Player.Look.canceled  += ctx => lookInput = Vector2.zero;
//     }

//     void OnEnable() {
//         actions.Player.Enable();
//     }
//     void OnDisable() => actions.Player.Disable();

//     void Start()
//     {
//         cameraStartPos = cameraTransform.localPosition;
//         Cursor.lockState = CursorLockMode.Locked;

//         lookInput = Vector2.zero;
        
    
//         lookInputDisabled = true; // Block look input immediately
//         Invoke(nameof(EnableLookInput), 0.1f); // Wait 0.1 seconds (6 frames @ 60 FPS)

//         xRotation = cameraTransform.localEulerAngles.x;
//         if (xRotation > 180f) xRotation -= 360f; // convert 270 -> -90
//         cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//     }
    
//     private void EnableLookInput()
//     {
//         lookInputDisabled = false;
//         lookInput = Vector2.zero; // Clean up any events that snuck through during the delay
//         Debug.Log("Look input enabled and stabilized."); 
//     }

//     void Update()
//     {
//         Debug.Log("Look input: " + lookInput);
        
//         HandleLook();
//         HandleMovement();
//         HandleHeadBob();
//     }

//     void HandleLook()
//     {
//         if (lookInputDisabled) return; // Ignores all input for the first 0.1 seconds

//         // Checks if the movement magnitude is impossibly high (e.g., > 100 units/frame)
//         if (lookInput.sqrMagnitude > (100f * 100f)) 
//         {
//             Debug.LogWarning($"Ignoring massive look input spike: {lookInput.magnitude}.");
//             lookInput = Vector2.zero; // Eat the bad input
//             return; 
//         }

//         if (lookInput.sqrMagnitude < 0.0001f) return; // skip rotation if no input
        
//         Vector2 delta = lookInput * mouseSensitivity * Time.deltaTime;

//         xRotation -= delta.y;
//         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//         cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//         transform.Rotate(Vector3.up * delta.x);
//     }


//     void HandleMovement()
//     {
//         Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

//         if (controller.isGrounded && verticalVelocity < 0)
//             verticalVelocity = -2f; // keep grounded

//         verticalVelocity += gravity * Time.deltaTime;
//         move.y = verticalVelocity;

//         controller.Move(move * walkSpeed * Time.deltaTime);
//     }

//     void HandleHeadBob()
//     {
//         if (controller.isGrounded && moveInput.magnitude > 0.1f)
//         {
//             bobTimer += Time.deltaTime * bobFrequency;
//             float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
//             cameraTransform.localPosition = cameraStartPos + new Vector3(0, bobOffset, 0);
//         }
//         else
//         {
//             bobTimer = 0f;
//             cameraTransform.localPosition = Vector3.Lerp(
//                 cameraTransform.localPosition, cameraStartPos, Time.deltaTime * 5f);
//         }
//     }
// }

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float gravity = -9.81f;
    // Added for if we want to manually modify the player's mouse sens
    public float mouseSensitivity = 100f;

    public float bobFrequency = 8f;
    public float bobAmplitude = 0.05f;
    // Slight camera sway
    public float swayAmount = 0.05f;
    private Vector3 swayOffset;

    public Transform cameraTransform;
    private bool lookInputDisabled = true; 

    private CharacterController controller;
    private InputSystem_Actions actions;
    private float verticalVelocity;
    private float xRotation;

    private float bobTimer;
    private Vector3 cameraStartPos;

    // Input values
    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 horizontalVelocity;
    public float acceleration = 6f; // Can modify
    public float deceleration = 6f; // Can modify

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = new InputSystem_Actions();

        // Move input
        actions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        // Look input
        actions.Player.Look.performed += ctx => {
            lookInput = ctx.ReadValue<Vector2>();
        };
        actions.Player.Look.canceled  += ctx => lookInput = Vector2.zero;
    }

    void OnEnable() {
        actions.Player.Enable();
    }
    void OnDisable() => actions.Player.Disable();

    void Start()
    {
        cameraStartPos = cameraTransform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;

        lookInput = Vector2.zero;
        
    
        lookInputDisabled = true; // Block look input immediately
        Invoke(nameof(EnableLookInput), 0.1f); // Wait 0.1 seconds (6 frames @ 60 FPS)

        xRotation = cameraTransform.localEulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f; // convert 270 -> -90
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    private void EnableLookInput()
    {
        lookInputDisabled = false;
        lookInput = Vector2.zero; // Clean up any events that snuck through during the delay
        Debug.Log("Look input enabled and stabilized."); 
    }

    void Update()
    {
        Debug.Log("Look input: " + lookInput);
        
        HandleLook();
        HandleMovement();
        HandleHeadBob();
    }

    void HandleLook()
    {
        if (lookInputDisabled) return; // Ignores all input for the first 0.1 seconds

        // Checks if the movement magnitude is impossibly high (e.g., > 100 units/frame)
        if (lookInput.sqrMagnitude > (100f * 100f)) 
        {
            Debug.LogWarning($"Ignoring massive look input spike: {lookInput.magnitude}.");
            lookInput = Vector2.zero; // Eat the bad input
            return; 
        }

        if (lookInput.sqrMagnitude < 0.0001f) return; // skip rotation if no input
        
        Vector2 delta = lookInput * mouseSensitivity * Time.deltaTime;

        xRotation -= delta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * delta.x);
    }


    void HandleMovement()
    {
        Vector3 targetMove = transform.right * moveInput.x + transform.forward * moveInput.y;
        targetMove *= walkSpeed;

        // Smooth acceleration/deceleration
        horizontalVelocity.x = Mathf.Lerp(horizontalVelocity.x, targetMove.x, (moveInput.magnitude > 0.1f ? acceleration : deceleration) * Time.deltaTime);
        horizontalVelocity.z = Mathf.Lerp(horizontalVelocity.z, targetMove.z, (moveInput.magnitude > 0.1f ? acceleration : deceleration) * Time.deltaTime);

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        horizontalVelocity.y = verticalVelocity;

        controller.Move(horizontalVelocity * Time.deltaTime);

    }

    void HandleHeadBob()
    {
        if (controller.isGrounded && moveInput.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            // Lateral sway is always perpendicular to forward direction
            float lateralSway = Mathf.Sin(bobTimer/2 - Mathf.PI / 4) * swayAmount;

            // Combine vertical bob + lateral sway
            cameraTransform.localPosition = cameraStartPos + new Vector3(lateralSway, bobOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraStartPos, Time.deltaTime * 5f);
        }
    }
}



