using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSeedsOnDeath : MonoBehaviour
{
    public GameObject seedPrefab;
    public float maxDropDistance;
    public int seedAmount = 3;


    private void Start()
    {
        GetComponent<Health>().healthDepleted.AddListener(DropSeeds);
    }



    public void DropSeeds()
    {
        for(int i = 0; i< seedAmount; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-maxDropDistance, maxDropDistance), 0f, Random.Range(-maxDropDistance, maxDropDistance));
            GameObject.Instantiate(seedPrefab, transform.position, Quaternion.identity);
        }
            
    }
}
