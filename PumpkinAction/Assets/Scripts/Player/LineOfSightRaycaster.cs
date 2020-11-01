using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightRaycaster : MonoBehaviour
{

    int turretLayerMask = 1 << 15;
    public float turretMenuDistance = 5f;
    Team team;
    PlayerStats stats;
    UpgradeMenu currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        team = GetComponentInParent<TeamTag>().team;
        stats = GetComponentInParent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,transform.forward,out hit,turretMenuDistance, turretLayerMask))
        {
            UpgradeMenu menu = hit.collider.GetComponentInParent<UpgradeMenu>();

            if (menu == null)
                return;
            menu.OpenMenu(team);

            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Trying to upgrade turret");
                menu.Upgrade(ref stats.seedCount);
            }

        }

    }
}
