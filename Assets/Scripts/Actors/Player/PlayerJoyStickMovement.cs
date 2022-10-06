using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script responsible for taking joystick movement and converting coordinates into
 * 2D Char controller script
 */

public class PlayerJoyStickMovement : MonoBehaviour
{
    [SerializeField]
    private MobileJoyStick joystick;
    public Vector2 MovementVector { get; private set; }
    public CharacterController2D controller;
    public float runSpeed = 150f;


    private void Move(Vector2 input)
    {
        MovementVector = input;
        controller.Move(input.x * runSpeed * Time.fixedDeltaTime, false);
    }

    private void FixedUpdate()
    {
        Move(MovementVector);
    }

    // Start is called before the first frame update
    void Start()
    { 
        joystick.OnMove += Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
