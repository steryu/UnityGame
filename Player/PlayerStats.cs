using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class PlayerStats
{
    // default base stats

    // these can be permenatly upgrade through the shop (maybe affitnity)

    // defensive
    public float BaseHealth = 400f;
    public float TemporaryHealth = 0f;

    public float HealthRegeneration = 1f;
    public float BaseDefense = 10f;
    public float DodgeChance = 1f;
    // offensive
    public float BaseDamage = 10f;
    public float AttackSpeed = 1f;
    public float AttackRange = 5f;
    // critical
    public float CriticalHit = 0f;
    public float CriticalDamage = 2f;
    public float CriticalHitChance = 5f;
    // miscellane
    public float MovementSpeed = 10f;
    public float GoldGain = 0;
    public float ExperienceGain = 0f;
    public float PickupRadius = 0f;



    public enum PassiveType
    {
        Health,
        Damage,
        // Add other upgrade types here if needed
    }

    public void AddTemporaryUpgrade(PassiveType type, float amount)
    {
        switch (type)
        {
            case PassiveType.Health:
                TemporaryHealth += amount;
                break;
            // Add cases for other temporary upgrades if needed
            default:
                Debug.LogWarning("Unhandled upgrade type: " + type);
                break;
        }
    }

    public void AddHealth(float amount)
    {
        BaseHealth += amount;
        // save
    }

    /*   private void SaveStats()
       {

       }*/

    // load


    // updated base status
}
