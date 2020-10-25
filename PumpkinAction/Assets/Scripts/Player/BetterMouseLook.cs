using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterMouseLook : MonoBehaviour
{
    public float Xsensitivity = 3.0f;
    public float Ysensitivity = 3.0f;

    public float crouchHeight = 1.5f;
    private float standHeight;

    public Camera playercam;
    public playerInput playerInput;
    public GameObject playerBase;
    public GameObject cameraAnchor;

    public GameObject playerCharacter;
    public LayerMask firstPersonCullingLayer;

    float horizontal;
    float vertical;

    float pitch_angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        standHeight = cameraAnchor.transform.position.y - playerBase.transform.position.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //TODO put this in onStartLocalPlayer in a networked game.
        firstPersonCullingLayer = LayerMask.NameToLayer("FirstPersonCulling");

        SetLayerRecursively(playerCharacter.gameObject, firstPersonCullingLayer);
    }

    //moving JUST the camera based on framerate should be OK because the character movement is happening independently
    //also you wouldnt want the camera to lag behind for very high FPS computers
    void Update()
    {
        horizontal = playerInput.mouse_X * Xsensitivity; //yaw
        vertical = -playerInput.mouse_Y * Ysensitivity; //inverted pitch

        pitch_angle = Mathf.Clamp(pitch_angle + vertical, -90f, 90f);
        //Debug.Log("pitch_angle:" + pitch_angle);
        //Debug.Log("horizontal:" + horizontal);

        //setting the pitch angle directly means the value is correctly wrapped between -90 and 90 degrees
        //instead of skipping from 0-90 and 360-270 as the transform.localEulerAngles shows
        cameraAnchor.transform.localEulerAngles = new Vector3(pitch_angle, 0, 0);

        //Debug.Log("local angles:" + transform.localEulerAngles);

        playerBase.transform.Rotate(Vector3.up * horizontal);


        //CROUCHING (change camera height)
        if (playerInput.crouch_axis)
        {
            cameraAnchor.transform.localPosition = Vector3.up * crouchHeight;
        }
        else
        {
            cameraAnchor.transform.localPosition = Vector3.up * standHeight;
        }
    }


    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }

            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
