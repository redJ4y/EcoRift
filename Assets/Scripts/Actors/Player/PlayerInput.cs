using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeReference] private ProjectileHandler handler;
    [SerializeField] private MobileJoyStick joystick;
    
    public PlayerControls controls;
    public CharacterController2D controller;
    public float runSpeed = 30f;

    private bool usingNewInput = false;
    private float newInputDirection = 0f;
    private float joystickDirection = 0f;
    private bool jump = false;

    void Start()
    {
        joystick.OnMove += JoystickMove;
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Ground.Run.performed += ctx =>
        {
            newInputDirection = ctx.ReadValue<float>();
            usingNewInput = true;
        };

        controls.Ground.Jump.started += ctx =>
        {
            jump = true;
        };

        controls.Ground.Shoot.performed += ctx =>
        {
            handler.OnShoot();
        };
    }

    // Called a fixed number of times per second
    void FixedUpdate()
    {
        if (usingNewInput)
        {
            controller.Move(newInputDirection * runSpeed * Time.fixedDeltaTime, jump);
        }
        else
        {
            controller.Move(joystickDirection * runSpeed * Time.fixedDeltaTime, jump);
        }

        jump = false;
    }

    private void JoystickMove(Vector2 input)
    {
        joystickDirection = input.normalized.x;
    }
}
