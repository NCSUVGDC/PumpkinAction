using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;
    public bool lockedToY = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }


    void LateUpdate()
    {
        transform.LookAt(cam.transform);
        if (lockedToY)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
