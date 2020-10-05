using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100;
    float currentHealth;

    [SerializeField]
    Text healthIndicator = null;
    [SerializeField] bool isBoss=false;

    // Added by Bman:

    public delegate void ActorInjury();

    public event ActorInjury OnTookDamage;
    public AudioClip[] hurtClip;
    
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
        OnOnTookDamage();
        UpdateHealthIndicator();
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die()
    {
        if (gameObject.GetComponent<PlayerController>())
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
        }
        else if (isBoss) {
            SoundManager.instance.TransitionBackgroundMusic(SoundManager.BackgroundMusic.Victory);
            Destroy(gameObject, 0);
        }
        //For now
        else Destroy(gameObject, 0);
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    protected virtual void OnOnTookDamage()
    {
        if (hurtClip.Length>0) GetComponent<AudioSource>().PlayOneShot(hurtClip[UnityEngine.Random.Range(0,hurtClip.Length)]);
        OnTookDamage?.Invoke();
    }
}
