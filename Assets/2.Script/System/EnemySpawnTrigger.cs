using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.InGame;

public class EnemySpawnTrigger : MonoBehaviour
{
    [Header("# Main Data")]
    public int spawnCount = 30;
    [SerializeField] Transform[] _spawnPoints;
    GameObject _target;

    bool hasTriggerd = false;

    void OnTriggerEnter(Collider col)
    {
        if (hasTriggerd) return;

        if (col.CompareTag("Player"))
        {
            hasTriggerd = true;
            _target = col.gameObject;
            SpawnZombieWave();
        }
    }

    void SpawnZombieWave()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            Transform zombie = PoolManager.Instance.Get(PoolManager.Instance.Zombie).transform;
            zombie.position = spawnPoint.position;
            zombie.parent = PoolManager.Instance.transform.Find("EnemyPool");

            // TODO 좀비들의 Target을 영구적으로 지정해줘야함(Random으로)
            zombie.GetComponent<Enemy>().SetTargetFromEvent(_target);
        }
    }
}
