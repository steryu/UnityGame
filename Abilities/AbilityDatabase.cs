using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDatabase : MonoBehaviour
{
    public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();
    public Dictionary<string, Ability> UnlockedAbilitiesDictionary = new Dictionary<string, Ability>();

    private void Start()
    {
        LoadAbilities();
    }

    private void LoadAbilities()
    {
        Ability[] loadedAbilities = Resources.LoadAll<Ability>("Abilities");
        foreach (Ability ability in loadedAbilities)
        {
            if (!abilityDictionary.ContainsKey(ability.Name))
            {
                abilityDictionary.Add(ability.Name, ability);
            }
            else
            {
                Debug.Log("Ability with name " + ability.Name + " already exists in the database.");
            }
        }
    }
    public void addAbility(Ability ability)
    {
        if (!abilityDictionary.ContainsKey(ability.Name)) 
        {
            abilityDictionary.Add(ability.Name, ability);
        }
        else
        {
            Debug.Log("That ability is already in the database");
        }
    }

    public void RemoveAbility(string name)
    {
        if (abilityDictionary.ContainsKey(name))
        {
            abilityDictionary.Remove(name);
        }
        else
        {
            Debug.Log("There is no such ability");
        }
    }

    public Ability GetAbility(string name)
    {
        if (abilityDictionary.ContainsKey(name))
        {
            return (Ability)abilityDictionary[name];
        }
        else
        {
            Debug.Log("Ability not found in the database");
            return null;
        }
    }

    public List<Ability> GetAbilities()
    {
        return new List<Ability>(abilityDictionary.Values);
    }    
    
    public List<Ability> GetUnlockedAbilities()
    {
        return new List<Ability>(abilityDictionary.Values);
    }

    public int GetUnlockedAbilitiesLength()
    {
        return abilityDictionary.Count;
    }
}
