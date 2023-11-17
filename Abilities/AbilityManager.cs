using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using static PlayerStats;

public class AbilityManager : MonoBehaviour
{
	public AbilityDatabase m_Database;
	public List<Ability> arsenal = new List<Ability>();
	public Transform playerTransform;

	public void setPassiveUpgrade(Ability ability)
	{
        PlayerHealth playerHealth = GameObject.Find("Witch").GetComponent<PlayerHealth>();
        PassiveType type;

        if (Enum.TryParse(ability.BaseName, out type))
        {
/*            AddTemporaryUpgrade(type, ability.subStats.Damage);*/
			playerHealth.NewHealth(ability.subStats.Damage);
        }
    }

	public void AddAbilityToArsenal(Ability ability)
	{
		RemovePreviousUpgrade(ability);

		Ability newAbility = Instantiate(ability);
		arsenal.Add(newAbility);

		GameObject instantiatedObject = Instantiate(newAbility.Prefab, playerTransform);
		newAbility.SetInstantiatedObject(instantiatedObject);

		if (ability.Level < ability.Upgrades.Length)
		{
			Ability UpgradedAbility = Instantiate(ability);
			AbilityUpgrade upgrade = ability.Upgrades[ability.Level];
			UpgradedAbility.ApplyUpgrade(upgrade);

			m_Database.addAbility(UpgradedAbility);
			m_Database.RemoveAbility(ability.Name); // lock
		}
		if (ability.Level == ability.Upgrades.Length)
		{
			m_Database.RemoveAbility(ability.Name); // lock
			Debug.Log("Removed: " + ability.Name);
		}
/*		AllArsenalAbilities();*/
	}

	void RemovePreviousUpgrade(Ability ability)
	{
		Ability previousUpgradedAbility = arsenal.Find(a => a.BaseName == ability.BaseName && a.Level < ability.Level);
		if (previousUpgradedAbility != null)
		{
			arsenal.Remove(previousUpgradedAbility);
			if (previousUpgradedAbility.instantiatedObject != null)
			{
                Destroy(previousUpgradedAbility.instantiatedObject);
            }
		}
	}

	public Ability GetAbilityInArsenal(string name)
	{
		foreach (Ability e in arsenal)
		{
			if (e.BaseName == name)
			{
				return (e);
			}
		}
		return null;
	}

	public void AllArsenalAbilities()
	{
		foreach (Ability ability in arsenal)
		{
			Debug.Log("ARSENAL: " + ability + "Name: " + ability.Name + " Damage: " + ability.subStats.Damage + " Level: " + ability.Level);
		}
	}

	void Update()
	{
		Cooldown();
	}

	void Cooldown()
	{
		foreach (Ability ability in arsenal)
		{
			if (ability.IsOnCooldown) // cooldown
			{
				ability.cooldownTimer -= Time.deltaTime;
				if (ability.cooldownTimer <= 0f)
				{
					ability.IsOnCooldown = false;
					ability.resetActiveTimer = true;
				}
			}
			else // activate
			{
				if (ability.resetActiveTimer)
				{
					ability.activeTimer = 0f;
					ability.resetActiveTimer = false;
				}

				ability.activeTimer += Time.deltaTime;
				if (ability.activeTimer >= ability.subStats.ActiveTime)
				{
					ability.IsOnCooldown = true;
					ability.cooldownTimer = ability.subStats.CooldownTime;
				}
			}
		}
	}
}

