using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePhysics : MonoBehaviour
{
    public Rigidbody playerRigidBody;
    public Transform playerBase;

    public playerInput playerInput;
    public groundCheck groundchecker;

    public float ForwardSpeedMultiplier = 1.3f; //sprint speed multiplier

    public float playerWalkSpeed = 4.35f;
    public float playerWalkMultiplier = 1.0f; //change this to change the base speed in-game via multipliers
    private float playerWalkSpeed_saved;

    public float CrouchSpeedMultiplier = 0.5f;
    private float crouch_AppliedMultiplier;

    public float groundFriction = 0.66f;
    public float airWalkSpeedMultiplier = 0.04f;
    public float terminalVelocity = 40f;
    public float airDrag = 0.02f; //inverse percentage multiplier
    public float gravity = -65f;

    private Vector2 walk_velocity;
    private float vertical_velocity;
    private Vector3 curr_velocity;

    private Vector3 move_force = Vector3.zero;

    public bool onGround;
    public bool DoubleJumpEnabled = true;
    private bool canDoubleJump;
    private bool alreadyDoubleJumped;

    public float jumpHeight = 1.25f;

    private Vector2 walkInput;

    private bool jump_axis;
    private bool crouch_axis;
    private bool sprint_axis;

    public float jumpTimer = 0.5f;
    private float jumpTimer_manip;
    private bool jumped;
    private bool queued_jump;

    private bool debugMode;

    private void Start()
    {
        playerWalkSpeed_saved = playerWalkSpeed;
        jumpTimer_manip = jumpTimer;
    }

    // Update is called once per frame
    void Update()
    {
        walkInput = new Vector2(playerInput.horizontal_axis, playerInput.vertical_axis);
        walkInput = Vector2.ClampMagnitude(walkInput, 1f); //The magnitude of move speed input should always be 1 to prevent holding strafe and forward being significantly faster. 

        jump_axis = playerInput.jump_axis;
        crouch_axis = playerInput.crouch_axis;

        sprint_axis = playerInput.sprint_axis;

        //cheat code for changing jump height
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            jumpHeight += 0.25f;
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            jumpHeight = Mathf.Clamp(jumpHeight - 0.25f, 0.0f, Mathf.Infinity); //can't be less than 0
        }

        debugMode = playerInput.debugMode;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DoubleJumpEnabled = !DoubleJumpEnabled;
        }
    }
    private void OnGUI() //hud for debug mode (F12)
    {
        if (debugMode)
        {
            GUILayout.BeginArea(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, 1000, 500));
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperLeft;

            GUILayout.Label("Velocity XYZ: " + Mathf.Round(playerRigidBody.velocity.x * 100f) / 100f +" , " + Mathf.Round(playerRigidBody.velocity.y * 100f) / 100f + " , " + Mathf.Round(playerRigidBody.velocity.z * 100f) / 100f);
            GUILayout.Label("Velocity Magnitude: " + Mathf.Round(playerRigidBody.velocity.magnitude * 100f) / 100f);
            GUILayout.Label("Forward Magnitude: " + Mathf.Round(Vector3.Project(curr_velocity, playerBase.forward).magnitude * 100f) / 100f);
            GUILayout.Label("Horizontal Magnitude: " + Mathf.Round(Vector3.Project(curr_velocity, playerBase.right).magnitude * 100f) / 100f);
            GUILayout.Label("X: " + Mathf.Round(walkInput.y * 100f) / 100f + " Y: " + Mathf.Round(walkInput.x * 100f) / 100f + " Jump Timer: " + Mathf.Round(jumpTimer_manip * 100f) / 100f);
            GUILayout.Label("Grounded: " + onGround + " , Queued Jump: " + queued_jump + " , Jump Height: " + jumpHeight + " , Currently Sprinting: " + sprint_axis);
            GUILayout.Label("Double Jump Enabled: " + DoubleJumpEnabled);

            GUILayout.EndArea();
        }
    }
    private void FixedUpdate()
    {
        onGround = groundchecker.onGround;

        //use the playerbase angles for move direction, but move the entire player
        curr_velocity = playerRigidBody.velocity;
        Vector3 curr_velocity_forward = Vector3.Project(curr_velocity, playerBase.forward);
        Vector3 curr_velocity_strafe = Vector3.Project(curr_velocity, playerBase.right);

        //apply any base-speed modifier
        playerWalkSpeed = (playerWalkMultiplier != 1.0f ? playerWalkSpeed_saved * playerWalkMultiplier : playerWalkSpeed_saved);


        //Jump timer
        //holding down jump is slower than spamming the jump button
        if (jump_axis)
        {
            if (!jumped && jumpTimer_manip <= 0f)
            {
                jumped = true;
                //jumpTimer_manip = jumpTimer; //this should get set after successfully jumping
            }
            else if (jumpTimer_manip > 0f)
            {
                jumpTimer_manip -= Time.deltaTime;
                jumped = false;
            }
        }
        else
        {
            jumped = false;
            jumpTimer_manip = 0f;
        }

        crouch_AppliedMultiplier = (crouch_axis ? CrouchSpeedMultiplier : 1f); //apply crouch slowness

        ////ON GROUND///
        if (onGround) //Movement while on the ground
        {
            alreadyDoubleJumped = false;
            canDoubleJump = false;


            walk_velocity.y = walkInput.y; //forward/back
            walk_velocity.x = walkInput.x; //strafing

            if (walkInput.y >= 0 && sprint_axis) //only apply sprint multiplier if sprinting and trying to move forward
            {
                walk_velocity.y *= ForwardSpeedMultiplier;
            }


            float applied_groundFriction = groundFriction;
            if (jumped)
            {
                vertical_velocity = Mathf.Sqrt(-1f * jumpHeight * gravity); // ??? NOT v = square_root( -2 * g * h )
                jumpTimer_manip = jumpTimer;
                //Debug.Log("Jump Velocity: " + vertical_velocity);
                if (queued_jump)
                {
                    applied_groundFriction = 1.0f; //dont apply friction if we are landing after queing a jump in the air
                    queued_jump = false;
                }
            }
            else
            {
                vertical_velocity = curr_velocity.y; //dont set to 0 while jumping
                
                queued_jump = false;
            }

            //Debug.Log(applied_groundFriction);

            walk_velocity *= playerWalkSpeed * crouch_AppliedMultiplier;


            //apply forward/back
            //curr_velocity_forward = (curr_velocity_forward + (playerBase.forward * walk_velocity.y)) * applied_groundFriction;
            curr_velocity_forward = (curr_velocity_forward + playerBase.forward * walk_velocity.y * applied_groundFriction) - (curr_velocity_forward * applied_groundFriction);
            //apply left/right
            curr_velocity_strafe = (curr_velocity_strafe + playerBase.right * walk_velocity.x * applied_groundFriction) - (curr_velocity_strafe * applied_groundFriction);

            //build the player velocity vector for ground movement
            //also includes current vertical velocity to prevent eating momentum changes
            curr_velocity = curr_velocity_forward + curr_velocity_strafe;
            curr_velocity = new Vector3(curr_velocity.x, vertical_velocity, curr_velocity.z);
        }

        ////NOT ON GROUND////
        if (!onGround)
        {
            walk_velocity.y = walkInput.y; //apply forward/back
            walk_velocity.x = walkInput.x; //apply left/right

            //Debug.Log("X: " + walk_velocity.y + " Y: " + walk_velocity.x);

            //increased movement multiplier when reducing air momentum that stops after a threshold
            float cutMomentumFactor = 3f; //slowing speed multiplier in air until 0.02 speed
            float applied_airDrag = (sprint_axis && walkInput.y > 0.001f ? airDrag : airDrag * 1.5f); //hacky different drag value whether sprinting or not

            float airDragX = (Mathf.Abs(walkInput.y) > 0.001f ? applied_airDrag : 0.1f );//slowing speed multiplier in air when not pressing anything
            float airDragZ = (Mathf.Abs(walkInput.x) > 0.001f ? airDrag * 1.5f : 0.1f );//slowing speed multiplier in air when not pressing anything

            //Debug.Log("Forward: " + Vector3.Angle(curr_velocity_forward, playerBase.forward * walkInput.y) + " Strafe: " + Vector3.Angle(curr_velocity_horizontal, playerBase.right * walkInput.x));

            //cut forwards/backwards momentum
            if (curr_velocity_forward.magnitude > 0.02f && Vector3.Angle(curr_velocity_forward, playerBase.forward * walkInput.y) > 90)
            {
                walk_velocity.y *= cutMomentumFactor;
            }
            //cut right/left momentum
            if (curr_velocity_strafe.magnitude > 0.02f && Vector3.Angle(curr_velocity_strafe, playerBase.right * walkInput.x) > 90)
            {
                walk_velocity.x *= cutMomentumFactor;
            }

            walk_velocity *= airWalkSpeedMultiplier * playerWalkSpeed * crouch_AppliedMultiplier;
            //walk_velocity = Vector2.ClampMagnitude(walk_velocity, playerWalkSpeed * airWalkSpeedMultiplier * cutMomentumFactor);

            //only apply gravity to vertical velocity if not on the ground
            //otherwise a very large y velocity builds up
            //no need to use air drag here since this equation handles the speed
            vertical_velocity = curr_velocity.y + gravity / 100; //linear? gravity v1 = v0 + v
            vertical_velocity = Mathf.Clamp(vertical_velocity, -terminalVelocity, Mathf.Infinity); //clamp vertical velocity with terminal velocity

            //build the player velocity vector for free fall
            curr_velocity_forward += walk_velocity.y * playerBase.forward;
            curr_velocity_strafe += walk_velocity.x * playerBase.right;

            Vector3 targetWalkVelocity = (curr_velocity_forward * (1 - airDragX) + curr_velocity_strafe * (1 - airDragZ));

            curr_velocity = new Vector3(targetWalkVelocity.x, vertical_velocity, targetWalkVelocity.z);

            if (jumped)
            {
                queued_jump = true;
            }

            //handling double jump (override curr_velocity)
            if (!jump_axis && !alreadyDoubleJumped && DoubleJumpEnabled)
            {
                canDoubleJump = true;
            }

            if (jump_axis && canDoubleJump && DoubleJumpEnabled)
            {
                canDoubleJump = false;
                alreadyDoubleJumped = true;

                //apply gravity
                vertical_velocity = Mathf.Sqrt(-1f * jumpHeight * gravity); // ??? NOT v = square_root( -2 * g * h )
                //Debug.Log("Double Jumped!");

                //build the player velocity vector for double jump
                targetWalkVelocity = playerBase.forward * walkInput.y + playerBase.right * walkInput.x;
                targetWalkVelocity *= playerWalkSpeed * ForwardSpeedMultiplier;

                curr_velocity = new Vector3(targetWalkVelocity.x, vertical_velocity, targetWalkVelocity.z);
                //Debug.Log(targetWalkVelocity);
            }
        }

        //apply final velocity vector
        playerRigidBody.velocity = curr_velocity;
    }
}
