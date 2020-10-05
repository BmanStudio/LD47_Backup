using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{

    float damage=1;
    GameObject shooter = null;
    [SerializeField] private float timeToDie = 3;
    public float Damage { get => damage; set => damage = value; }
    public GameObject Shooter { get => shooter; set => shooter = value; }

    void Start()
    {
        Destroy(this.gameObject, timeToDie);
    }
    void OnTriggerEnter(Collider other)
    {
        //Make sure that when this is fire it will stop at obsticles OR player.
       
        HealthSystem hs = other.GetComponent<HealthSystem>();
        if (hs != null && other.gameObject != Shooter) {
            hs.TakeDamage(damage);
        }
        if(!other.isTrigger && other.gameObject != Shooter) {
            Destroy(this.gameObject, 0);
        }

    }
}