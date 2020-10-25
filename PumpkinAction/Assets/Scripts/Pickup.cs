using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool? pickedUp = other.gameObject.GetComponentInParent<PlayerStats>()?.AddSeed();

            if (pickedUp == true)
            {
                PickItUp();
            }
            else if (pickedUp == null)
            {
                Debug.LogWarning("PlayerStats was not found when picking up seed");
            }
        }
    }

    void PickItUp()
    {
        Debug.Log("Picked Up");
        //play sound
        audioManager.Play("Test");

        GameObject.Destroy(gameObject);
    }
}
