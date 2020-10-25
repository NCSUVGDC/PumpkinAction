using System;
using System.CodeDom;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public enum Level
{
    Level1,
    Level2,
    Level3
}

public enum TurretType
{
    chunker,
    minipumpkin,
    webShooter
}

public class Turret : MonoBehaviour
{

    public GameObject RotatingPart;
    public float range = 5f;


    [Header("Shooting Settings")]
    public float ShotCooldownTime = .2f;
    public Transform BarrelTip;
    public float projectileGravity = -9.81f;
    public float xRotationVariance = 0f;
    public float yRotationVariance = 0f;

    //Team
    private TeamTag teamTag;
    private Team[] attackTeams;

    [Header("Turret Stats")]
    public TurretType turretType;
    public Level rangeUpgrade;
    public Level fireRateUpgrade;
    public Level damageUpgrade;
    public Level healthUpgrade;

    [Header("Projectile Prefabs")]
    public GameObject Chunker_DamageLevel1;
    public GameObject Chunker_DamageLevel2;
    public GameObject Chunker_DamageLevel3;

    public GameObject Minipumpkin_DamageLevel1;
    public GameObject Minipumpkin_DamageLevel2;
    public GameObject Minipumpkin_DamageLevel3;

    public GameObject WebShooter_DamageLevel1;
    public GameObject WebShooter_DamageLevel2;
    public GameObject WebShooter_DamageLevel3;

    [Header("Chunker")]

    [Header("Turret Upgrade Values")]

    public float[] chunker_range_level = new float[] { 10f, 20f, 25f };
    public float[] chunker_fireRate_level = new float[] { 3f, 2f, 1f };
    public float[] chunker_damage_level = new float[] { 10f, 20f, 25f };
    public int[] chunker_health_level = new int[] { 100, 200, 500 };
    public float chunker_variance = .1f;


    [Header("Minipumpkin")]

    public float[] minipumpkin_range_level = new float[] { 5f, 10f, 15f };
    public float[] minipumpkin_fireRate_level = new float[] { .5f, .2f, .05f };
    public float[] minipumpkin_damage_level = new float[] { 1f, 3f, 5f };
    public int[] minipumpkin_health_level = new int[] { 50, 100, 300 };
    public float minipumpkin_variance = .6f;

    [Header("Web Shooter")]

    public float[] webshooter_range_level = new float[] { 10f, 20f, 25f };
    public float[] webshooter_fireRate_level = new float[] { 10f, 20f, 25f };
    public float[] webshooter_damage_level = new float[] { 10f, 20f, 25f };
    public int[] webshooter_health_level = new int[] { 100, 200, 500 };
    public float webshooter_variance = .1f;

    bool readyToShoot = true;
    bool active = true;
    LayerMask enemies;
    private AudioManager audioManager;
    private Vector3 shotVariance;
    private GameObject projectilePrefab;

    //upgrade listeners
    private TurretType previousType;
    private Level previousRange;
    private Level previousDamage;
    private Level previousFireRate;
    private Level previousHealth;

    private Health health;




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

    }

    //Call this whenever you upgrade the turret
    void ChangeProjectileType ()
    {
        switch (turretType)
        {
            case TurretType.chunker:
                switch (damageUpgrade)
                {
                    case Level.Level1:
                        projectilePrefab = Chunker_DamageLevel1;
                        break;
                    case Level.Level2:
                        projectilePrefab = Chunker_DamageLevel2;
                        break;
                    case Level.Level3:
                        projectilePrefab = Chunker_DamageLevel3;
                        break;
                    default:
                        break;
                }
                break;
            case TurretType.minipumpkin:
                switch (damageUpgrade)
                {
                    case Level.Level1:
                        projectilePrefab = Minipumpkin_DamageLevel1;
                        break;
                    case Level.Level2:
                        projectilePrefab = Minipumpkin_DamageLevel2;
                        break;
                    case Level.Level3:
                        projectilePrefab = Minipumpkin_DamageLevel3;
                        break;
                    default:
                        break;
                }
                break;
            case TurretType.webShooter:
                switch (damageUpgrade)
                {
                    case Level.Level1:
                        projectilePrefab = WebShooter_DamageLevel1;
                        break;
                    case Level.Level2:
                        projectilePrefab = WebShooter_DamageLevel2;
                        break;
                    case Level.Level3:
                        projectilePrefab = WebShooter_DamageLevel3;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    void ApplyTurretUpgrade()
    {
        switch (turretType)
        {
            case TurretType.chunker:
                ChangeProjectileType();
                range = chunker_range_level[(int)rangeUpgrade];
                ShotCooldownTime = chunker_fireRate_level[(int)fireRateUpgrade];
                if(previousHealth != healthUpgrade)
                    health.SetMaxHealth(chunker_health_level[(int)healthUpgrade]);
                xRotationVariance = chunker_variance;
                yRotationVariance = chunker_variance;
                break;
            case TurretType.minipumpkin:
                ChangeProjectileType();
                range = minipumpkin_range_level[(int)rangeUpgrade];
                ShotCooldownTime = minipumpkin_fireRate_level[(int)fireRateUpgrade];
                if (previousHealth != healthUpgrade)
                    health.SetMaxHealth(minipumpkin_health_level[(int)healthUpgrade]);
                xRotationVariance = minipumpkin_variance;
                yRotationVariance = minipumpkin_variance;
                break;
            case TurretType.webShooter:
                ChangeProjectileType();
                range = webshooter_range_level[(int)rangeUpgrade];
                ShotCooldownTime = webshooter_fireRate_level[(int)fireRateUpgrade];
                if (previousHealth != healthUpgrade)
                    health.SetMaxHealth(minipumpkin_health_level[(int)healthUpgrade]);
                xRotationVariance = webshooter_variance;
                yRotationVariance = webshooter_variance;
                break;
            default:
                break;
        }

        previousDamage = damageUpgrade;
        previousFireRate = fireRateUpgrade;
        previousRange = rangeUpgrade;
        previousHealth = healthUpgrade;
        previousType = turretType;
    }
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.healthDepleted.AddListener(OnHealthDepleted);
        health.healthChanged.AddListener(OnHealthChanged);

        teamTag = GetComponent<TeamTag>();

        //Set who enemies are so it doesn't shoot at team members
        string[] teamNames = Enum.GetNames(typeof(Team));
        ArrayList enemies = new ArrayList(0);

        foreach(string teamName in teamNames)
        {
            if(teamName != teamTag.team.ToString() && teamName != Team.Neutral.ToString())
            {
                enemies.Add(teamName);
            }
        }
        this.enemies = LayerMask.GetMask((string[])enemies.ToArray(typeof(string)));
        audioManager = FindObjectOfType<AudioManager>();

        ApplyTurretUpgrade();

    }

    // Update is called once per frame
    void Update()
    {

        if (damageUpgrade != previousDamage || fireRateUpgrade != previousFireRate || previousRange != rangeUpgrade || previousType != turretType)
        {
            ApplyTurretUpgrade();
        }

            if (active)
            {
                GameObject trueEnemy = null; //Target the closest enemy
                float trueEnemyDistance = range;
                Collider[] hitColliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 10, 0), transform.position + new Vector3(0, -10, 0), range, enemies);
                foreach (var enemy in hitColliders)
                {
                    Debug.Log("Found an enemy " + enemy.gameObject.name);
                    var enemyDistance = (transform.position - enemy.transform.position).magnitude;
                    if (enemyDistance < trueEnemyDistance)
                    {
                        trueEnemy = enemy.gameObject;
                        trueEnemyDistance = enemyDistance;
                    }
                }
                if (trueEnemy != null)
                {
                    Debug.Log("Looking towards enemy");
                    RotatingPart.transform.LookAt(trueEnemy.transform.position);
                    RotatingPart.transform.rotation = Quaternion.Euler(0f, RotatingPart.transform.rotation.eulerAngles.y, 0f);

                    if (readyToShoot)
                    {
                        //Fire
                        readyToShoot = false;
                        audioManager.Play("Test");

                        float calcVelocity = Mathf.Sqrt((trueEnemyDistance - 1) * Mathf.Abs(projectileGravity)); //V = Sqrt(dist * grav) when shooting at 45 degrees

                        shotVariance = new Vector3(Random.Range(-xRotationVariance, xRotationVariance), Random.Range(-yRotationVariance, yRotationVariance), 1);

                        GameObject projectile = GameObject.Instantiate(projectilePrefab, BarrelTip.position, BarrelTip.rotation);
                        Rigidbody projectileBody = projectile.GetComponent<Rigidbody>();
                        projectileBody.velocity = calcVelocity * (BarrelTip.TransformVector(shotVariance));
                        projectileBody.useGravity = false;
                        projectileBody.GetComponent<CustomGravity>().gravity = new Vector3(0, projectileGravity, 0);

                        //start cooldown
                        StartCoroutine(ShotCooldown());
                    }
                }
            }
        }


        IEnumerator ShotCooldown()
        {
            yield return new WaitForSeconds(ShotCooldownTime);
            readyToShoot = true;
               
        }

    void OnHealthDepleted()
    {
        Debug.Log(this.gameObject.name + " health depleted");
        active = false;
    }

    void OnHealthChanged()
    {
        
    }
}
