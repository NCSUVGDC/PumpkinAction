using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilder : MonoBehaviour
{

    public Camera playerCam;
    public float turretPlacementDistance = 3;

    public GameObject turretPrefab;
    public Team team;
    public int turretCost = 8;
    public PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        team = GetComponent<TeamTag>().team;
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.seedCount > turretCost)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, turretPlacementDistance))
                {
                    Debug.Log("Possible to place turret");
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        Debug.Log("Placing Chunker");
                        stats.seedCount -= turretCost;
                        GameObject turret = GameObject.Instantiate(turretPrefab, hit.point + new Vector3(0, .5f, 0), Quaternion.identity);
                        Turret turretStats = turret.GetComponent<Turret>();
                        turretStats.turretLevel = Level.Level0;
                        turretStats.turretType = TurretType.chunker;
                        turretStats.GetComponent<TeamTag>().team = team;
                        Debug.Log("Spawned turret for " + team.ToString());

                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        Debug.Log("Placing minipumpkin");
                        stats.seedCount -= turretCost;
                        GameObject turret = GameObject.Instantiate(turretPrefab, hit.point + new Vector3(0, .5f, 0), Quaternion.identity);
                        Turret turretStats = turret.GetComponent<Turret>();
                        turretStats.turretLevel = Level.Level0;
                        turretStats.turretType = TurretType.minipumpkin;
                        turretStats.GetComponent<TeamTag>().team = team;
                    }
                }
            }
        }

    }
}
