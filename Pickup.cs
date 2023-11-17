using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private PlayerExp _playerExp;
    [SerializeField] bool isHealth;
    [SerializeField] public float expValue;
    [SerializeField] private float healValue;

    private void Start()
    {
        _playerExp = GameObject.Find("Witch").GetComponent<PlayerExp>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHealth == true)
        {
            /*other.GetComponent<PlayerHealth>().Heal(healValue);*/
        }
        if (_playerExp != null)
        {
            _playerExp.setExp(expValue);
            Destroy(gameObject);
        }
        else
            Debug.Log("no pickup found");
    }
}
