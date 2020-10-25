using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int seedCount = 0;
    [SerializeField]
    private int seedCapacity = 10;

    public Team team;

    Health health;


    private HUDManager HUD;
    /*
     * returns false if can't carry seed
     */
    public bool AddSeed()
    {
        if (seedCount < seedCapacity)
        {
            seedCount++;
            HUD.updateSeedCount(seedCount, seedCount == seedCapacity);
            return true;
        }
        else
            return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        HUD = GetComponentInChildren<HUDManager>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
