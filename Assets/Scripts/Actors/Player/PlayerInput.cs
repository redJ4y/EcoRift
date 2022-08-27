using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerControls controls;
    public CharacterController2D controller;
    public float runSpeed = 30f;

    private float playerDirection = 0f;
    private bool jump = false;

    private void Awake()
    {
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

    // Called a fixed number of times per second
    void FixedUpdate()
    {
        controller.Move(playerDirection * runSpeed * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
