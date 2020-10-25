using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        body.AddForce(gravity * body.mass);
    }

    
}
