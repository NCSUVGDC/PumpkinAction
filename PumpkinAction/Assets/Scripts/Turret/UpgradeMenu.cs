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


    private Team team;
    private int seedCostForNextUpgrade;
    private Turret turret;

    private void Start()
    {
        team = GetComponent<TeamTag>().team;
        turret = GetComponent<Turret>();
        header.text = turret.turretType.ToString();
        seedCostText.text = "x " + turret.upgradeCost[(int)turret.turretLevel];
        levelIndicator.sprite = levelIndicators[(int)turret.turretLevel];
    }

    //When a player looks at the turret call this
    public void OpenMenu(Team team)
    {
        if(team == this.team)
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }
        
    }

    private void Update()
    {
        menu.SetActive(false);
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
            seedCostText.text = "x " + turret.upgradeCost[(int)turret.turretLevel];
            levelIndicator.sprite = levelIndicators[(int)turret.turretLevel];

        }
    }
}
