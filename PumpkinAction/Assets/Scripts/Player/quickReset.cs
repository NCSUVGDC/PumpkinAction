using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickReset : MonoBehaviour
{
    public Rigidbody playerRigidBody;
    public playerInput playerInput;

    public Vector3 resetPosition;

    public float yVoidValue = -20f;

    // Start is called before the first frame update
    void Start()
    {
        resetPosition = playerRigidBody.transform.position; //set the reset position to be where the gameobject starts
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.reset)
        {
            ResetPlayer();
        }
    }
    private void FixedUpdate()
    {
        if (playerRigidBody.position.y <= yVoidValue)
        {
            ResetPlayer();
        }

    }

    public void ResetPlayer()
    {
        playerRigidBody.transform.position = resetPosition;
        playerRigidBody.velocity = Vector3.zero;
    }
}
