using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickReset : MonoBehaviour
{
    public Rigidbody playerRigidBody;
    public playerInput playerInput;

    public Vector3 resetPosition;

    public float yVoidValue = -20f;

    private Health health;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        resetPosition = playerRigidBody.transform.position; //set the reset position to be where the gameobject starts
        stats = GetComponent<PlayerStats>();
        health = GetComponent<Health>();
        health.healthDepleted.AddListener(ResetPlayer);
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
        health.Heal(health.maxHealth);
        stats.seedCount = 0;
    }
}
