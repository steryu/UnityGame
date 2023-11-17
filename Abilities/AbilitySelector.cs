using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    [SerializeField] private AbilityManager abilityManager;
    [SerializeField] private AbilityDatabase database;
    [SerializeField] private AbilitySelectorButton[] _abilityButton;

    public void Setup()        
    {
        if (database.GetUnlockedAbilitiesLength() == 0)
        {
            Debug.Log("no abilities to display");
        }
        else
        {
            DisplayAbilities();
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void DisplayAbilities()
    {
        // 3 random abilities
        List<Ability> abilities = database.GetAbilities();
        if (abilities == null)
        {
            Debug.Log("error getting receiving abilities list");
        }
        if (abilities.Count > 0)
        {
            foreach (var abilityButton in _abilityButton)
            {
                var ability = abilities[UnityEngine.Random.Range(0, abilities.Count)]; // error when no abilities left
                abilityButton.Init(ability.Name, ability.Description);
                abilityButton.Button.onClick.AddListener(() =>
                {
                    if (ability.isPassive)
                        abilityManager.setPassiveUpgrade(ability);
                    else
                        abilityManager.AddAbilityToArsenal(ability);
                    SetInactive();
                });
            }
        } 
        else
        {
            Debug.Log("no abilities left to display");
        }
    }

    public void SetInactive()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
