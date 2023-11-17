using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AppUI.UI;
using UnityEditor.Playables;
using UnityEngine;

public class MagicOrb: MonoBehaviour
{
	public GameObject orbPrefab;
	private AbilityManager _abilityManager;
	private Transform _playerTransform;

	[SerializeField] private Ability abilityData;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float radius = 3f;
	private int numberOfOrbs;

	Ability AbilityState;
	private float angle = 0.0f;

	private void Start()
	{
		if (transform.parent != null)
			_playerTransform = transform.parent;
		else
			_playerTransform = transform;

		_abilityManager = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
		if (_abilityManager == null)
		{
			Debug.Log("No abilityManager script found.");
		}

		AbilityState = _abilityManager.GetAbilityInArsenal(abilityData.BaseName);
		if (AbilityState == null)
		{
			Debug.Log("Magic Ability not found");
		}
		InstantiateOrbs();
	}

	private void InstantiateOrbs()
	{
		numberOfOrbs = AbilityState.subStats.Amount;
		for (int i = 0; i < numberOfOrbs; i++)
		{
			float angleRadians = Mathf.Deg2Rad * (i * (360f / numberOfOrbs));
			float x = Mathf.Cos(angleRadians) * radius;
			float z = Mathf.Sin(angleRadians) * radius;

			// Set the Y-coordinate to 1 to position the orbs at the same height as the player
			Vector3 orbPosition = _playerTransform.position + new Vector3(x, 1, z);

			// Instantiate and position the orb
			GameObject newOrb = Instantiate(orbPrefab, orbPosition, Quaternion.identity, transform);

			OrbCollisionHandler orbCollisionHandler = newOrb.GetComponent<OrbCollisionHandler>();
			if (orbCollisionHandler != null)
			{
				orbCollisionHandler.SetDamageData(AbilityState.subStats.Damage);
			}
		}
	}

	void Update()
	{
		angle += rotationSpeed * Time.deltaTime;

		for (int i = 0; i < transform.childCount; i++)
		{
			float angleRadians = Mathf.Deg2Rad * ((i * (360 / numberOfOrbs))  + angle);
			float x = Mathf.Cos(angleRadians) * radius;
			float z = Mathf.Sin(angleRadians) * radius;

			Vector3 orbPosition = _playerTransform.position + new Vector3(x, 1, z);

			transform.GetChild(i).position = orbPosition;
		}
	}
}