﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LeftPlayer : MonoBehaviour
{
    //public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    //public bool cursorVisible = false;
    public static int strikeL = 0; //no action
    [Header("Movement")]
    public float speed = 4;

    CharacterController controller;
    Vector3 movement, finalMovement;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        //Cursor.lockState = cursorLockMode;
        //Cursor.visible = cursorVisible;
    }

    void FixedUpdate()
    {
        var translation = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.A))
        {
            strikeL = 1; //rock //Up
            translation = new Vector3(0, 0, -60);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            strikeL = 2; //paper //Down
            translation = new Vector3(0, 0, -60);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            strikeL = 3; //scissor //Thrust
            translation = new Vector3(0, 0, -60);
        }
        movement = transform.TransformDirection(translation * speed * -1);
        finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime);
        controller.Move(finalMovement * Time.fixedDeltaTime);
    }
}
