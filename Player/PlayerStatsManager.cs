using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats PlayerStats;
/*    public static PlayerStatsManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize player stats (you might want to load from a save file here)
            PlayerStats = new PlayerStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    public float BaseHealth => PlayerStats.BaseHealth;
    public float TemporaryHealth => PlayerStats.TemporaryHealth;
    public float HealthRegeneration => PlayerStats.HealthRegeneration;
    public float BaseDefense => PlayerStats.BaseDefense;
    public float DodgeChance => PlayerStats.DodgeChance;
    public float BaseDamage => PlayerStats.BaseDamage;

    public float MaxHealth
    {
        get { return BaseHealth + TemporaryHealth; }
    }

}

