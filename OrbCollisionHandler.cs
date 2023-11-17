using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollisionHandler : MonoBehaviour
{
    private float _damage;
    public void SetDamageData(float newDamage)
    {
        _damage = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyData enemy = other.GetComponent<EnemyData>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}
