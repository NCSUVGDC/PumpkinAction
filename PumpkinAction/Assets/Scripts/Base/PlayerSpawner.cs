using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public List<GameObject> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform RequestSpawnpoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)].transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
