using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInput : MonoBehaviour
{
    public float vertical_axis;
    public float horizontal_axis;
    public float mouse_X;
    public float mouse_Y;
    public bool jump_axis;
    public bool crouch_axis;
    public bool sprint_axis;
    public bool attack;
    public bool attack2;
    public bool attack3;
    public bool aimDownSights;
    public bool reload;

    public bool reset;

    public bool debugMode = false;

    // Update is called once per frame
    void Update()
    {
        horizontal_axis = Input.GetAxisRaw("Horizontal");
        vertical_axis = Input.GetAxisRaw("Vertical");

        mouse_X = Input.GetAxis("Mouse X");
        mouse_Y = Input.GetAxis("Mouse Y");

        jump_axis = Input.GetButton("Jump");

        crouch_axis = Input.GetKey(KeyCode.LeftControl); //Ideally a button input in the project settings input manager

        sprint_axis = Input.GetKey(KeyCode.LeftShift); //Ideally a button input in the project settings input manager

        attack = Input.GetButton("Fire1"); //left click
        attack2 = Input.GetButton("Fire2"); //right click
        attack3 = Input.GetButton("Fire3"); //middle click

        aimDownSights = Input.GetKey(KeyCode.Q); //Ideally a button input in the project settings input manager
        reload = Input.GetKey(KeyCode.R); //Ideally a button input in the project settings input manager

        if (Input.GetKeyDown(KeyCode.F12))
        {
            debugMode = !debugMode;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        reset = Input.GetKeyDown(KeyCode.R); //Ideally a button input in the project settings input manager
    }
}
