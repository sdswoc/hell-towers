using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WaveSpawnner : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    private NetworkObject SpawnedNetworkEnemy;
    private GameObject enemyInstance;

    public override void OnNetworkSpawn()
    {
        if(!IsServer || enemy == null)
        {
            return;
        }
        InvokeRepeating("Generate", 2f, 2f);
    }

    void Generate()
    {
        enemyInstance = Instantiate(enemy);
        enemyInstance.transform.position = transform.position;
        enemyInstance.transform.rotation = transform.rotation;

        SpawnedNetworkEnemy = enemyInstance.GetComponent<NetworkObject>();
        SpawnedNetworkEnemy.Spawn();
    }

}
