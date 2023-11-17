using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject statsPanel;
    public static bool GameIsPause = false;
    public static bool StatsIsVisable = false;
    public GameObject pauseMenuUI;

    TextMeshProUGUI[] statTexts;

    private void Start()
    {
        statTexts = statsPanel.GetComponentsInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!StatsIsVisable)
                ShowStatsPanel();
            else
                HideStatsPanel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
                Resume();
            else
                Pause();
        }   
    }

    void ShowStatsPanel()
    {
        statsPanel.SetActive(true);
        InitStatsPanel();
        Time.timeScale = 0f;
        StatsIsVisable = true;
    }

    void HideStatsPanel()
    {
        statsPanel.SetActive(false);
        Time.timeScale = 1f;
        StatsIsVisable = false;
    }
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading menu...");
        SceneManager.LoadScene("MainMenu");
    }

    void ExitGame() 
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    void InitStatsPanel()
    {
/*        GetStatText("Max Health", PlayerStatsManager.Instance.MaxHealth);
        GetStatText("Health Regeneration", PlayerStatsManager.Instance.HealthRegeneration);
        GetStatText("Defense", PlayerStatsManager.Instance.BaseDefense);
        GetStatText("Damage", PlayerStatsManager.Instance.BaseDamage);*/
    }

    void GetStatText(string statName, float statValue)
    {
        TextMeshProUGUI statText = statTexts.FirstOrDefault(text => text.name == statName);

        if (statText != null)
            statText.text = $"{statName} {statValue}";
    }
}
