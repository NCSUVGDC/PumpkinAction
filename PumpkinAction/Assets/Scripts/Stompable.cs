using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompable : MonoBehaviour
{
    Health health;
    public AudioClip stompSound;

    private void Start()
    {
        health = GetComponentInParent<Health>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(stompSound, transform.position);
        health.ApplyDamage(100);
    }
}
