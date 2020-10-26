using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    public UnityEvent healthDepleted;
    public UnityEvent healthChanged;
    public UnityEvent maxHealthChanged;



    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            healthDepleted.Invoke();
        }
        healthChanged.Invoke();
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(int health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        healthChanged.Invoke();
    }

    //Auto heals
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;

        healthChanged.Invoke();
        maxHealthChanged.Invoke();
    }
}
