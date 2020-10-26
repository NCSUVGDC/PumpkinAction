using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{
    AudioManager audioManager;
    
    public float splashRadius;
    public float damage;

    TeamTag teamTag;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        teamTag = GetComponent<TeamTag>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioManager.Play("Test");

        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, splashRadius);
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();
        foreach(Collider col in objectsInRange)
        {
            
            TeamTag otherTag = col.GetComponentInParent<TeamTag>();

            Debug.Log("Shot " + col.gameObject.name);
            if (otherTag != null && !hitObjects.Contains(col.gameObject))
            {
                hitObjects.Add(col.gameObject);


                if (otherTag.team != teamTag.team) //Only damage other teams
                {
                    Health enemy = col.GetComponentInParent<Health>();
                    if (collision.collider == col)
                    {
                        Debug.Log("Direct hit on " + col.gameObject.name);
                        enemy.ApplyDamage((int)damage);
                        continue;
                    }

                    if (enemy != null)
                    {
                        // linear falloff of effect
                        float proximity = (this.transform.position - col.transform.position).magnitude;
                        if (proximity < splashRadius)
                        {
                            float effect = 1 - (proximity / splashRadius);
                            Debug.Log("Applying " + ((int)damage * effect) + " damage to " + enemy.gameObject.name);
                            enemy.ApplyDamage((int)(damage * effect));
                        }
                    }
                }
            }
        }

        GameObject.Destroy(this.gameObject);
    }
}
