using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public float menuActiveRadius = 5f;
    public Canvas menu;

    private Team team;
    private bool lookedAtThisFrame = false;
    private void Start()
    {
        team = GetComponent<TeamTag>().team;

    }

    //When a player looks at the turret call this
    public void OpenMenu(Team team)
    {
        if(team == this.team)
        {
            lookedAtThisFrame = true;
            return;
        }
    }

    private void LateUpdate()
    {
        menu.enabled = lookedAtThisFrame;
        lookedAtThisFrame = false;
    }
}
