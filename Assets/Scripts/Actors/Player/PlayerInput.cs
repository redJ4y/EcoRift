using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private MobileJoyStick joyStick;
    public Vector2 MovementVector { get; private set; }
    public PlayerControls controls;
    public CharacterController2D controller;
    public float runSpeed = 30f;
    public event Action<Vector2> OnMovement;
    private float playerDirection = 0f;
    private bool jump = false;

    private void Awake()
    {
        joyStick.OnMove += Move;
        controls = new PlayerControls();
        controls.Enable();
        controls.Ground.Run.performed += ctx =>
        {
            playerDirection = ctx.ReadValue<float>();
        };
        controls.Ground.Jump.performed += ctx =>
        {
            jump = true;
        };
    }

    private void Move(Vector2 input)
    {
        MovementVector = input;
        controller.Move(input.x * runSpeed * Time.fixedDeltaTime, false, jump);
    }

    // Called a fixed number of times per second
    void FixedUpdate()
    {
        Move(MovementVector);
        //controller.Move(playerDirection * runSpeed * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
