using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Ability", menuName = "New Ability/Basic Ability")]
public class Ability : ScriptableObject
{
	public GameObject Prefab;
	public string BaseName;
	public string Name;
	public string Description;
	public int Level;
	public bool isPassive;

	public SubStats subStats;
	[System.Serializable]
	public class SubStats
	{
        public float Damage;
        public float ActiveTime;
		public float CooldownTime;
		public int Amount;
	}

	public bool isUnlocked;
	public AbilityUpgrade[] Upgrades;

	[HideInInspector]
	public bool IsOnCooldown;
	[HideInInspector]
	public GameObject instantiatedObject;
	[HideInInspector]
	public float cooldownTimer;
	[HideInInspector]
	public float activeTimer;
	[HideInInspector]
	public bool resetActiveTimer;
	[HideInInspector]
	public Vector3 newPosition;

	private void OnEnable()
	{
		isUnlocked = false;
		IsOnCooldown = false;
		cooldownTimer = 0.0f;
		activeTimer = 0.0f;
		resetActiveTimer = false;
	}

	public void SetInstantiatedObject(GameObject obj)
	{
		instantiatedObject = obj;
	}

	public void ApplyUpgrade(AbilityUpgrade upgrade)
	{
			ApplyUpgradeEffect(upgrade);
	}

	protected virtual void ApplyUpgradeEffect(AbilityUpgrade upgrade)
	{
		// ability upgrade effects in the overrride function for each ability 
	}
}

