using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public  SpawnEnemy spawner;
    void Start()
    {
        
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.EnemyKilled();
        }
    }


    void Update()
    {
        
    }
}
