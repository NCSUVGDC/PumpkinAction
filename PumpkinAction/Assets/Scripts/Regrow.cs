using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regrow : MonoBehaviour
{
    public float regrowTime = 15f;
    public GameObject visual;

    private Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.healthDepleted.AddListener(StartRegrowing);
    }


    void StartRegrowing()
    {
        visual.SetActive(false);
        StartCoroutine("regrowTimer");
    }

    IEnumerator regrowTimer()
    {
        yield return new WaitForSeconds(regrowTime);
        visual.SetActive(true);
        health.Heal(health.maxHealth);
    }
}
