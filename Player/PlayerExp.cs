 using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerExp : MonoBehaviour
{
    public AbilitySelector abilitySelector;
    [SerializeField] public float playerEXP;
    [SerializeField] private EXPBar _EXPBar;
    public TextMeshProUGUI levelText;
    [SerializeField] public int playerLevel = 1;

    private void Start()
    {
        playerLevel = 1;
        levelText.text = "LEVEL " + playerLevel;
    }

    public void setExp(float exp)
    {
        playerEXP += exp;
        _EXPBar.UpdateEXPBar(playerEXP);
        if (_EXPBar.CheckLevelUp(playerEXP) == true)
        {
            LevelUp();
            abilitySelector.Setup();
            playerEXP = 0;
        }
    }

    void LevelUp()
    {
        playerLevel++;
        levelText.text = "Level " + playerLevel;
        PlayerPrefs.SetInt("Level", playerLevel);
    }
}
