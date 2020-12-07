using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Michael Schmidt

public class EnemySpawner : NetworkBehaviour
{

    [SerializeField]
    private GameObject[] m_enemy;

    [SerializeField]
    private float m_spawnTimer;

    [SerializeField]
    float spawnerCount;

    public override void OnStartServer()
    {
        spawnerCount = Random.Range(0, 35);
        InvokeRepeating("SpawnEnemy", m_spawnTimer, m_spawnTimer);
        base.OnStartServer();
    }

    void SpawnEnemy()
    {
        if (spawnerCount == 3)
        {
            GameObject enemy = Instantiate(m_enemy[Random.Range(0, m_enemy.Length)], transform.position, Quaternion.identity);

            NetworkServer.Spawn(enemy);
        }
    }
}
