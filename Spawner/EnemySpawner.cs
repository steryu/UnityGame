using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawner", menuName = "Enemy Spawner/Enemy Type")]
public class EnemySpawner : ScriptableObject
{
    public GameObject prefab;
    public float spawnTime;
    public Vector2 spawnRange;
    public float spawnInterval;
/*    [SerializeField] private float _initialDelay;*/
    public int amount;
}
