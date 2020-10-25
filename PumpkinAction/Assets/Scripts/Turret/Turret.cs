using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public GameObject RotatingPart;
    public float range = 5f;


    [Header("Shooting Settings")]
    public float ShotCooldownTime = .2f;
    public GameObject ProjectilePrefab;
    public Transform BarrelTip;
    public float projectileVelocity = 3f;

    bool readyToShoot = true;
    LayerMask enemies;
    private AudioManager audioManager;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = LayerMask.GetMask("Team1", "Team2", "Team3", "Team4");
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject trueEnemy = null; //Target the closest enemy
        float trueEnemyDistance = range;
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 10, 0), transform.position + new Vector3(0, -10, 0), range, enemies);
        foreach(var enemy in hitColliders)
        {
            Debug.Log("Found an enemy " + enemy.gameObject.name);
            var enemyDistance = (transform.position - enemy.transform.position).magnitude;
            if (enemyDistance < trueEnemyDistance)
            {
                trueEnemy = enemy.gameObject;
                trueEnemyDistance = enemyDistance;
            }
        }
        if(trueEnemy != null)
        {
            Debug.Log("Looking towards enemy");
            RotatingPart.transform.LookAt(trueEnemy.transform);
            RotatingPart.transform.rotation = Quaternion.Euler(0f, RotatingPart.transform.rotation.eulerAngles.y, 0f);

            if (readyToShoot)
            {
                //Fire
                readyToShoot = false;
                audioManager.Play("Test");

                GameObject projectile = GameObject.Instantiate(ProjectilePrefab,BarrelTip.position, BarrelTip.rotation);
                projectile.GetComponent<Rigidbody>().velocity = projectileVelocity * BarrelTip.forward;



                //start cooldown
                StartCoroutine(ShotCooldown());
            }
        }



        IEnumerator ShotCooldown()
        {
            yield return new WaitForSeconds(ShotCooldownTime);
            readyToShoot = true;
               
        }
    }
}
