using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public float damage = 5;
    public bool rangedAttacks = false;
    public GameObject projectile =null;
    public float projectileSpeed=10;
    public float attacksPerSecond = 1.5f;
    float attackTimer = 0;
    public bool isBoss = false;
    GameObject player;
    bool isAttacking = false;
    
    public void StartAttackingPlayer(GameObject player) {
        this.player = player;
        isAttacking = true;
    }
    
    public void StopAttackingPlayer() {
        isAttacking = false ;
    }


    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attacksPerSecond && isAttacking) {
            Attack();
            attackTimer = 0;
        }
    }

    private void Attack()
    {
        if (!isBoss && player.GetComponent<Rigidbody>().velocity.magnitude == 0) return; //Only attacks moving targets, unless you're the boss :)
        if (!rangedAttacks) {
            Debug.Log("TODO: Indicate Melee attack");
            player.GetComponent<HealthSystem>().TakeDamage(damage);
        }
        else if(projectile!=null) {
            Debug.Log("TODO: Indicate Ranged attack");
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<BasicProjectile>().Damage = damage;
            bullet.GetComponent<BasicProjectile>().Shooter = gameObject;
            bullet.GetComponent<Rigidbody>().AddForce((player.transform.position- transform.position ).normalized * projectileSpeed);

        }
    }
}
