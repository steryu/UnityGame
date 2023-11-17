using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private Transform target;

    private void Start()
    {
        GameObject target2 = GameObject.Find("Witch");
        target = target2.transform;
    }
    void Update()
    {
        if (target != null && transform != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyData.enemySpeed * Time.deltaTime);
            transform.LookAt(targetPosition);
        }
        else
        {
            Debug.LogError("Target or transform is null.");
        }
    }
}
