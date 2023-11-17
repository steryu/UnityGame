using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberBallHolder : MonoBehaviour
{
    [SerializeField] private EmberBall _ballPrefab;
    [SerializeField] private float _fireSpeed;

    private Transform _playerTransform;
    private float _timer;
    private Spawner _spawner;

    private void Awake()
    {
        _playerTransform = transform.parent == null ? transform : transform.parent;
    }
    private void Start()
    {
        _spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        if (_spawner == null)
        {
            Debug.LogError("No Spawner script found.");
        }
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1 / _fireSpeed)
        {
            fireAtClosestEnemy();
            _timer = 0f;
        }
    }

    private void fireAtClosestEnemy()
    {
        // find closest enemy
        var closest = _spawner.GetClosestEnemyToPlayer(_playerTransform.position);
        if (closest != null)
        {
            var direction = closest.transform.position - _playerTransform.position;
            direction.y = 0f;
            FireProjectile(direction.normalized);
        }
    }

    private void FireProjectile(Vector3 directionNormalized)
    {
        var spawnPoint = _playerTransform.position + directionNormalized * 0.5f;
        spawnPoint.y = 1f;
        var projectile = Instantiate(_ballPrefab, spawnPoint, Quaternion.identity);
        projectile.Init(directionNormalized);
    }
}
