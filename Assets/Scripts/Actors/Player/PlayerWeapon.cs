﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public ParticleSystem fireEffect;
    public GameObject projectile;
    public float damage=20;
    public float projectileSpeed=100;
    public Transform muzzle;
    public float attackCooldown=0.5f;
    private float attackTimer = 0;
    bool canFire = true;
    
    
    // Added by Bman:
    private float startedDamage;

    void Awake()
    {
        startedDamage = damage;
    }

    public  void Fire(Vector3 direction) {
        if (muzzle != null &  fireEffect != null && projectile!=null && canFire)
        {
            fireEffect.Play();
            GameObject bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
            bullet.GetComponent<BasicProjectile>().Damage = damage;
            bullet.GetComponent<BasicProjectile>().Shooter = gameObject;
            bullet.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed);
            canFire = false;
            attackTimer = 0;


        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown)
        {
            canFire = true;
        }
       
    }

    // Added by Bman:
    public void UpdateDamagePassiveBonus(float value)
    {
        damage = startedDamage + value;
        Debug.Log("Updated the weapon attack power cuz of a passive bonus, now the power is " + damage);
    }
}