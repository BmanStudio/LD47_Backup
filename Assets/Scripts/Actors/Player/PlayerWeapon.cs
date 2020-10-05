using System.Collections;
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
    public AudioClip[] fireClips=null;
    
    
    // Added by Bman:
    private float startedDamage;

    void Awake()
    {
        startedDamage = damage;
    }

    public  void Fire(Vector3 direction,GameObject player) {
        if (muzzle != null & fireEffect != null && projectile != null && canFire)
        {
            fireEffect.Play();
            GameObject bullet = Instantiate(projectile, muzzle.position, muzzle.rotation);
            bullet.GetComponent<BasicProjectile>().Damage = damage;
            bullet.GetComponent<BasicProjectile>().Shooter = player;
            bullet.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed);
            canFire = false;
            attackTimer = 0;
            if (fireClips != null && fireClips.Length > 0) {
                player.GetComponent<AudioSource>().PlayOneShot(fireClips[Random.Range(0,fireClips.Length)]);
            }


        }
        else if(canFire){
            Debug.LogError("Attempting to use a weapon that isn't properly configured");
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
