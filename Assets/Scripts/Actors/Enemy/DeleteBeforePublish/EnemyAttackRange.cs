using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    EnemyAttacker attack;
    void Start()
    {
        attack = transform.parent.gameObject.GetComponent<EnemyAttacker>();
        if (attack == null) {
            Debug.LogError("Enemy have an attack range but no EnemyAttacking!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null){
            //Player entered!
            attack.StartAttackPlayer(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            //Player exitex!
            attack.StopAttackingPlayer();
        }
    }



}
