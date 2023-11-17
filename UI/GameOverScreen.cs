using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{

    public void Setup()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        SceneManager.LoadScene("GameOverScene");
    }
}
