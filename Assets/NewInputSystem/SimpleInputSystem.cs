using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleInputSystem : MonoBehaviour
{
    public float speed = 3, jumpPower = 10, gravity = 10;

    private float ySpeed;

    CharacterController characterController;

    InputAction jumpInput, moveInput;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumpInput = InputSystem.actions.FindAction("Jump");
        moveInput = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 input = moveInput.ReadValue<Vector2>();

        Vector3 movement = new(input.x, 0, input.y);

        movement *= speed;

        ySpeed -= gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            ySpeed = -1;

            if (jumpInput.WasPressedThisFrame())
            {
                ySpeed = jumpPower;
            }
        }

        movement.y = ySpeed;

        movement *= Time.deltaTime;

        characterController.Move(movement);
    }
}
