using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberBall : MonoBehaviour
{
    private AbilityManager _abilityManager;
    [SerializeField] private Ability abilityData;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    Ability AbilityState;

    public void Init(Vector3 direction)
    {
        transform.forward = direction;
        Invoke(nameof(Destroy), _lifeTime);
    }
    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Start()
    {
        
        _abilityManager = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
        if (_abilityManager == null)
        {
            Debug.Log("No abilityManager script found.");
        }
        AbilityState = _abilityManager.GetAbilityInArsenal(abilityData.BaseName);
        if (AbilityState == null)
        {
            Debug.Log("Fire Ability not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyData enemy = other.GetComponent<EnemyData>();
            if (enemy != null)
            {
                enemy.TakeDamage(AbilityState.subStats.Damage);
                Destroy();
            }
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
