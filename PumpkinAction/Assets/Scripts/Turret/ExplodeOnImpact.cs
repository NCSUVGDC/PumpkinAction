using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{

    public AudioClip explosionSound;
    [Range(0f,1f)]
    public float explosionSoundVolume = 1;
    public float splashRadius;
    public float damage;

    TeamTag teamTag;
    public ParticleSystem explosionEffect;
    public SpriteRenderer sprite;

    private void Start()
    {
        teamTag = GetComponent<TeamTag>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, splashRadius);
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();
        foreach(Collider col in objectsInRange)
        {
            
            TeamTag otherTag = col.GetComponentInParent<TeamTag>();

            if (otherTag != null && !hitObjects.Contains(col.gameObject))
            {
                hitObjects.Add(col.gameObject);


                if (otherTag.team != teamTag.team) //Only damage other teams
                {
                    Health enemy = col.GetComponentInParent<Health>();
                    if (collision.collider == col)
                    {
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
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = false;
        sprite.enabled = false;

        AudioSource.PlayClipAtPoint(explosionSound, this.transform.position,explosionSoundVolume);
        explosionEffect.Play();
        
        GameObject.Destroy(this.gameObject, 1f);
    }
}
