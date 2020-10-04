using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    float maxHealth=100;
    float currentHealth;

    [SerializeField]
    Text healthIndicator = null;

    // Added by Bman:
    private float startingMaxHealth;
    void Awake()
    {
        startingMaxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthIndicator();
    }

    public void TakeDamage(float damage) {
        currentHealth = Math.Max(0, currentHealth - damage);
        UpdateHealthIndicator();
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die()
    {
        //For now
        Destroy(gameObject, 0);
    }

    void UpdateHealthIndicator() {
        if (healthIndicator != null) {
            healthIndicator.text = "Health: " + currentHealth;
        }
    }

    // Added by Bman:
    public void UpdateHealthPassiveBonus(float value)
    {
        float relativeHealth = currentHealth / maxHealth;
        maxHealth = startingMaxHealth + value;
        currentHealth = maxHealth * relativeHealth;
        
        UpdateHealthIndicator();
        Debug.Log("Updated the max health cuz of a passive bonus, now the max is "
                  + maxHealth + " and the new relative health is " + currentHealth);
    }
    
}
