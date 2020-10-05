using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public GameObject enemiesSpawn;
    public int amountToSpawn=2;
    public float spawnDelay=5;
    public float area = 5;
    float spawnTimer = 3;
    bool animnim = false;

    public GameObject spawnOnDeathObject=null;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelay-1 && !animnim) {
            animator.SetBool("Taunt", true);
            animnim = true;
        }
        if (spawnTimer > spawnDelay) {
            
            for (int i = 0; i < amountToSpawn; i++)
            {
                Vector3 pos = transform.position;
                pos.x += Random.Range(-area, area);
                pos.y += Random.Range(-area, area);
                Instantiate(enemiesSpawn,pos,Quaternion.identity);
            }
            spawnTimer = 0;
            animnim = false;
        }

    }

    void OnDestroy()
    {
        if (spawnOnDeathObject) {
            Instantiate(spawnOnDeathObject, transform);
        
        }
    }
}
