using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public int seedCount = 0;
    [SerializeField]
    private int seedCapacity = 10;

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
        prevSeedCount = seedCount;
    }


    private int prevSeedCount;
    // Update is called once per frame
    void Update()
    {
        if(prevSeedCount != seedCount)
        {
            prevSeedCount = seedCount; //fixes count not going to zero
            HUD.updateSeedCount(seedCount, seedCount == seedCapacity);
        }
    }
}
