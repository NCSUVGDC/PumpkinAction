using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    AudioManager audioManager;
    
    public float splashRadius;
    public float damage;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioManager.Play("Test");

        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, splashRadius);
        foreach(Collider col in objectsInRange)
        {
            Health enemy = col.GetComponentInParent<Health>();

            if (enemy != null)
            {
                // linear falloff of effect
                float proximity = (this.transform.position - enemy.transform.position).magnitude;
                float effect = 1 - (proximity / splashRadius);


                enemy.ApplyDamage((int) (damage * effect));
            }
        }

        GameObject.Destroy(this.gameObject);
    }
}
