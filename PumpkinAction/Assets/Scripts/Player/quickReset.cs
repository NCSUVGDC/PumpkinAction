using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickReset : MonoBehaviour
{
    public Rigidbody playerRigidBody;
    public playerInput playerInput;

    public GameObject spectatorCamPrefab;

    public Vector3 resetPosition;

    public float yVoidValue = -20f;

    private Health health;
    private PlayerStats stats;
    private GameManager gm;
    private TeamTag teamTag;

    // Start is called before the first frame update
    void Start()
    {
        resetPosition = playerRigidBody.transform.position; //set the reset position to be where the gameobject starts
        stats = GetComponent<PlayerStats>();
        health = GetComponent<Health>();
        health.healthDepleted.AddListener(ResetPlayer);
        gm = FindObjectOfType<GameManager>();
        teamTag = GetComponent<TeamTag>();
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
        Debug.Log("Requesting spawn for " + teamTag.team);
        Transform spawnPoint = gm.requestSpawn(teamTag.team);
        if(spawnPoint != null)
        {
            playerRigidBody.transform.position = spawnPoint.position;
            playerRigidBody.transform.rotation = spawnPoint.rotation;
            playerRigidBody.velocity = Vector3.zero;
            health.Heal(health.maxHealth);
            stats.seedCount = 0;
        }
        else
        {
            Debug.Log("No respawn for you");
            GameObject specCam = GameObject.Instantiate(spectatorCamPrefab);
            Camera cam = GetComponentInChildren<Camera>();
            cam.transform.parent = specCam.transform;
            cam.transform.localPosition = Vector3.zero;
            Debug.Log("Should be destroyed");
            GameObject.Destroy(this.gameObject);


        }

    }
}
