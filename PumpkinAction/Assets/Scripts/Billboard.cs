using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;
    public bool lockedToY = true;
    public bool invert = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }


    void LateUpdate()
    {
        if (invert)
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        else
            transform.LookAt(cam.transform);
        if (lockedToY)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}
