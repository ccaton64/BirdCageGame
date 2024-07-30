using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    // Variable to store the characters animator
    Animator animator;

    // Variables to store getters and setters for IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingInPlaceHash;

    // Variable to store the instance of the PlayerInput
    PlayerInput input;

    // Variables for movment restricion
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    bool jumpPressed;

    // Awake is called when the script instance is loaded
    void Awake() 
    {
        input = new PlayerInput();

        // Set the player input values with listeners
        input.CharacterControls.Movement.performed += ctx =>{
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Movement.canceled += ctx => { movementPressed = false;};
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingInPlaceHash = Animator.StringToHash("isJumpingInPlace");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    // Handles rotation so that you can move multiple directions
    void HandleRotation()
    {
        // Current position of player character
        Vector3 currentPosition = transform.position;

        // Change in position player character should have
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        // Combined positoions to give look at
        Vector3 positionToLookAt = currentPosition + newPosition;

        // Rotate character to face the positionToLookAt
        transform.LookAt(positionToLookAt);
    }

    // Simple handler to assign bools of movement and act on them
    void HandleMovement()
    {
        //Get parameter values from the animator
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJumpingInPlace = animator.GetBool(isJumpingInPlaceHash);

        // Start walking if movement is pressed and not already walking
        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        // Stops walking if already walking and not pressing movment
        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Start running if movement is pressed and run is pressed but not already running
        if((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        // Stops running if movement or run not pressed and currently running
        if((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        if(jumpPressed && !isJumpingInPlace && !isWalking  && !isRunning)
        {
            animator.SetBool(isJumpingInPlaceHash, true);
        }

        if(!jumpPressed && isJumpingInPlace)
        {
            animator.SetBool(isJumpingInPlaceHash, false);
        }
    }

    // Handles slope movement by aligning character to surface
    

    // OnEnable is called when the script is enabled
    private void OnEnable()
    {
        // Enable the character controls action map
        input.CharacterControls.Enable();
    }

    // OnDisable is called when the script is disabled
    private void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
