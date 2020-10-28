using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightRaycaster : MonoBehaviour
{

    int turretLayerMask = 1 << 10;
    public float turretMenuDistance = 5f;
    Team team;

    UpgradeMenu currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        team = GetComponentInParent<TeamTag>().team;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward,out hit,turretMenuDistance))
        {

            UpgradeMenu menu = hit.collider.GetComponentInParent<UpgradeMenu>();

            menu.OpenMenu(team);

        }

    }
}
