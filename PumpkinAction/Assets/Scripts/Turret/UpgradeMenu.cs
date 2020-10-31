using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public float menuActiveRadius = 5f;
    public GameObject menu;

    public Text header;
    public Text seedCostText;
    public Image levelIndicator;
    public Sprite[] levelIndicators;
    public Image seedSprite;
    public Text healthText;

    private Health health;
    private Team team;
    private int seedCostForNextUpgrade;
    private Turret turret;

    private Level prevTurretLevel;

    private int prevHealth;
    private int prevMaxHealth;

    private void Start()
    {
        health = GetComponent<Health>();
        healthText.text = health.getCurrentHealth() + "/" + health.maxHealth;
        prevHealth = health.getCurrentHealth();
        prevMaxHealth = health.maxHealth;

        team = GetComponent<TeamTag>().team;
        turret = GetComponent<Turret>();
        header.text = turret.turretType.ToString();
        prevTurretLevel = turret.turretLevel;
        seedCostText.text = "x " + turret.upgradeCost[(int)turret.turretLevel];
        levelIndicator.sprite = levelIndicators[(int)turret.turretLevel];
    }
    bool lookedAtThisFrame = false;
    //When a player looks at the turret call this
    public void OpenMenu(Team team)
    {
        if(team == this.team)
        {
            Debug.Log("Making menu active");
            menu.SetActive(true);
            lookedAtThisFrame = true;
        }
        else
        {
            menu.SetActive(false);
        }
        
    }
    private void Update()
    {
        if(prevTurretLevel != turret.turretLevel)
        {
            //update menu visuals
            if(turret.turretLevel != Level.Level3)
                seedCostText.text = "x " + turret.upgradeCost[(int)turret.turretLevel];
            else
            {
                seedCostText.text = "MAX PUMPKIN CHUNKIN";
                seedSprite.enabled = false;
            }
            Debug.Log("changing visual to: " + (int)turret.turretLevel);
            levelIndicator.sprite = levelIndicators[(int)turret.turretLevel];
        }

        if(prevHealth != health.getCurrentHealth() || prevMaxHealth != health.maxHealth)
        {
            healthText.text = health.getCurrentHealth() + "/" + health.maxHealth;
            prevHealth = health.getCurrentHealth();
            prevMaxHealth = health.maxHealth;
        }

    }
    private void LateUpdate()
    {
        if(!lookedAtThisFrame)
        {
            menu.SetActive(false);
        }
        lookedAtThisFrame = false;
    }

    public void Upgrade(ref int seedCount)
    {
        if (turret.turretLevel == Level.Level3)
            return;
        int upgradeCost = turret.upgradeCost[(int)turret.turretLevel];
        if (seedCount >= upgradeCost)
        {
            seedCount -= upgradeCost;
            turret.turretLevel += 1;
            //Play upgrade effects 


        }
    }
}
