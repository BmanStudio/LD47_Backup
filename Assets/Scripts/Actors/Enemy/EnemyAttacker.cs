using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] float damage = 5f;
    [SerializeField] bool rangedAttacks = false;
    [SerializeField] GameObject _projectile = null;
    [SerializeField] float projectileSpeed = 500f;
    [SerializeField] public float FireCooldown = 1.5f;
    [SerializeField] private Transform _projectileStartPoint = null;

    //float _attackTimer = 0;
    Transform _player;
    //bool _isAttacking = false;

    void Start()
    {
        _player = PlayerInventory.Instance.transform;
    }
    
    public void StartAttackPlayer(GameObject player) {
        this._player = player.transform;
        //_isAttacking = true;
    }
    
    public void StopAttackingPlayer() {
        //_isAttacking = false ;
    }


    // Moved to the Brain
    /*public void Tick()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= attacksPerSecond) {
            Attack();
            _attackTimer = 0;
        }
    }*/

    public void Attack()
    {
        // Moved to EnemyAIBrain by Bman:
        //if (!isBoss && _player.GetComponent<Rigidbody>().velocity.magnitude == 0) return; //Only attacks moving targets, unless you're the boss :)
        
        
        if (!rangedAttacks) {
            Debug.Log("TODO: Indicate Melee attack");
            _player.GetComponent<HealthSystem>().TakeDamage(damage);
        }
        else if(_projectile!=null) {
            //Debug.Log("TODO: Indicate Ranged attack");
            GameObject bullet = Instantiate(_projectile, _projectileStartPoint.position, Quaternion.identity);
            bullet.GetComponent<BasicProjectile>().Damage = damage;
            bullet.GetComponent<BasicProjectile>().Shooter = gameObject;
            bullet.GetComponent<Rigidbody>().AddForce(((_player.position - new Vector3(0, 0.7f,0)) - transform.position).normalized * projectileSpeed);

        }
    }
}
