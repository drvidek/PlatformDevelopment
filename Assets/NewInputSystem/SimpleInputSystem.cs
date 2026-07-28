using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleInputSystem : MonoBehaviour
{
    public float speedWalk = 3, speedSprint = 7, jumpPower = 10, gravity = 10;

    private float ySpeed;

    CharacterController characterController;

    // InputAction - represents an input from an Input Map.
    // Can be pressed, held, released, or contain a value (e.g. Vector2)
    InputAction jumpInput, moveInput, sprintInput;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // InputSystem.actions represents the project-wide InputActionAsset
        // .FindAction() takes the name of the action (case sensitive)
        jumpInput = InputSystem.actions.FindAction("Jump");
        moveInput = InputSystem.actions.FindAction("Move");
        sprintInput = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        // .ReadValue<T>() will attempt to read the InputAction's contained value
        // You have to provide the correct type for the input
        Vector2 input = moveInput.ReadValue<Vector2>();

        // Map the input to a 3D movement vector
        Vector3 movement = new(input.x, 0, input.y);

        // Set a speed based on our sprint input
        // .IsPressed() is like .GetButton()
        movement *= sprintInput.IsPressed() ? speedSprint : speedWalk;

        // Apply gravity to our y speed
        ySpeed -= gravity * Time.deltaTime;

        // If the character controller is on the ground
        if (characterController.isGrounded)
        {
            // Set ySpeed to -1 (slight downward pull)
            ySpeed = -1;

            // .WasPressedThisFrame() is like .GetButtonDown()
            if (jumpInput.WasPressedThisFrame())
            {
                ySpeed = jumpPower;
            }
        }

        // Apply the ySpeed to our movement vector
        movement.y = ySpeed;

        // Make movement happen based on time
        movement *= Time.deltaTime;

        // Move the character controller
        characterController.Move(movement);
    }
}
