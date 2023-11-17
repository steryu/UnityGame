using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform _playerPostion;
    [SerializeField] private float _initialDelay = 1f;
    private float _currentangle;
    private bool _isActive;
    [SerializeField] Timer timer;

    // List to hold references to spawned enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public EnemySpawner[] enemySpawner;

    private List<float> spawnTimers = new List<float>();

    private IEnumerator Start()
    {
        _playerPostion = GameObject.Find("Witch").GetComponent<Transform>();
        foreach (var spawner in enemySpawner)
        {
            spawnTimers.Add(spawner.spawnInterval);
        }

        yield return new WaitForSeconds(_initialDelay);
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        for (int i = 0; i < enemySpawner.Length; i++)
        {
            var e = enemySpawner[i];
            spawnTimers[i] += Time.deltaTime;

            if (timer.elapsedTime > e.spawnTime && spawnTimers[i] >= e.spawnInterval)
            {
                for (int j = 0; j < e.amount; j++)
                {
                    GameObject spawnedEnemy = Spawn(e);
                    if (spawnedEnemy != null)
                    {
                        spawnedEnemies.Add(spawnedEnemy);
                    }
                }

                spawnTimers[i] = 0f;
            }
        }
    }

    GameObject Spawn(EnemySpawner e)
    {
        _currentangle += 180f + Random.Range(-45, 45);
        var angleInRad = _currentangle * Mathf.Rad2Deg;
        var range = Random.Range(e.spawnRange.x, e.spawnRange.y);
        var relativePosition = new Vector3(Mathf.Cos(angleInRad) * range, e.prefab.transform.position.y, Mathf.Sin(angleInRad) * range);
        var spawnPosition = _playerPostion.position + relativePosition;

        GameObject spawnedEnemy = Instantiate(e.prefab, spawnPosition, Quaternion.identity, transform);
        return spawnedEnemy;
    }

    public Transform GetClosestEnemyToPlayer(Vector3 playerPosition)
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(enemy.transform.position, playerPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }
        return closestEnemy;
    }
}
