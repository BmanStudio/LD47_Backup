using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public int maxSpawns=1;
    public GameObject enemy=null;
    void Start()
    {
       
        //For now we spawn all enemies at the begining, but it's wide enough to spawn more thn just that
        for (int i = 0; i < maxSpawns; i++)
        {
            //TODO/Maybe change the randomness
            int rngSpawnPoint = Random.Range(0,transform.childCount);
            SpawnAt(transform.GetChild(rngSpawnPoint).transform.position);
        }
        
    }

    void SpawnAt(Vector3 position) {
        Instantiate(enemy, position, Quaternion.identity);
    }
}
