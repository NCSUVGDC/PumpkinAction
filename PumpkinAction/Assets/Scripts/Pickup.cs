using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickItUp();
        }
    }

    void PickItUp()
    {
        Debug.Log("Picked Up");
        //play sound

        GameObject.Destroy(gameObject);
    }
}
