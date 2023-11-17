using System.Collections.Generic;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;

public class ExpAbsorberPickUp : MonoBehaviour
{
    private Transform _player;
    private PlayerExp _playerExp;
    private string _expOrbTag = "Exp";
    private float _attractionSpeed = 10f;
    private bool _collect = false;

    private int _amount = 0;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Witch");
        if (playerObject != null)
        {
            _player = playerObject.transform;
            _playerExp = playerObject.GetComponent<PlayerExp>();
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    private void Update()
    {
        if (_collect == true)
        {
            CollectAllExpOrbs();
        }
        if (_amount == 1)
            _collect = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Witch"))
        {
            _collect = true;
        }
    }

    private void CollectAllExpOrbs() // issue when orbs stop moveving when amount is reach. Rather want ony the orbs that are current on ground to move.
    {
        List<GameObject> expOrbsToDestroy = new List<GameObject>();
        GameObject[] expOrbs = GameObject.FindGameObjectsWithTag(_expOrbTag);

        _amount = expOrbs.Length;
        foreach (GameObject expOrb in expOrbs)
        {
            if (expOrb.CompareTag("Exp"))
            {
                PickupItem exp = expOrb.GetComponent<PickupItem>();

                // Calculate the direction to the player
                Vector3 position = new Vector3(_player.position.x, 1f, _player.position.z);
                Vector3 direction = position - expOrb.transform.position;

                float distance = direction.magnitude;
                // Normalize the direction to get a unit vector
                direction.Normalize();

                // Move the exp orb towards the player using lerp
                expOrb.transform.position += direction * Mathf.Min(_attractionSpeed * Time.deltaTime, distance);
                // Access the PickupItem component and collect its data

                if (distance < 0.1f)
                {
                    if (exp != null)
                    {
                        _amount--;
                        _playerExp.setExp(exp.expValue);
                        if (expOrb != null)
                        {
                            Destroy(expOrb.gameObject);
                        }

                    }
                }
            }
        }


        if (_amount == 1)
        {
            Destroy(gameObject);
        }
    }
}
