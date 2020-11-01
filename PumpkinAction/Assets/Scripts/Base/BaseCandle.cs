using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCandle : MonoBehaviour
{
    Health health;
    Team team;
    public ParticleSystem candleParticle;
    public Light candlelight;
    public Light roomLight;

    private float candleStartLifetime;
    private float candleStartSize;
    private float candleBrightness;
    private float roomBrightness;

    // Start is called before the first frame update
    void Start()
    {
        team = GetComponentInParent<TeamTag>().team;
        health = GetComponentInParent<Health>();
        health.healthDepleted.AddListener(BaseDestroyed);
        health.healthChanged.AddListener(CandleDamaged);

        candleStartLifetime = candleParticle.startLifetime;
        candleStartSize = candleParticle.startSize;
        candleBrightness = candlelight.intensity;
        roomBrightness = roomLight.intensity;
    }

    void BaseDestroyed()
    {
        FindObjectOfType<GameManager>().CandleDestroyed(team);
    }

    void CandleDamaged()
    {
        candleParticle.startLifetime = candleStartLifetime * ((float)health.getCurrentHealth() / (float)health.maxHealth);
        candleParticle.startSize = candleStartSize * ((float)health.getCurrentHealth() / (float)health.maxHealth);
        candlelight.intensity = candleBrightness * ((float)health.getCurrentHealth() / (float)health.maxHealth);
        roomLight.intensity = roomBrightness * ((float)health.getCurrentHealth() / (float)health.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
