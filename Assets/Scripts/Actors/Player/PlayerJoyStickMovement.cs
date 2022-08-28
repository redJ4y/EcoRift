using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoyStickMovement : MonoBehaviour
{
    [SerializeField]
    private MobileJoyStick joystick;
    public Vector2 MovementVector { get; private set; }
    public CharacterController2D controller;


    private void Move(Vector2 input)
    {
        MovementVector = input;
        
        
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
