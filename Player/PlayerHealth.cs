using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameOverScreen _gameOverScreen;
    public GameObject VFX_damage;
    private GameObject _currentVFX;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [SerializeField] private HealthBar _healthBar;
    void Start()
    {
/*        _maxHealth = PlayerStatsManager.Instance.MaxHealth;*/
        _currentHealth = _maxHealth;
        _healthBar.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    public void NewHealth(float addHealth)
    {   
        _currentHealth += addHealth;
/*        _maxHealth = PlayerStatsManager.Instance.MaxHealth;*/
        _healthBar.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBar.UpdateHealthBar(_maxHealth, _currentHealth);

        if (_currentHealth <= 0)
        {
            print(message: "you died!");
            _gameOverScreen.Setup();
        }

        // VFX Effect
/*        if (VFX_damage != null)
        {
            if (_currentVFX != null)
            {
                Destroy(_currentVFX);
            }
            _currentVFX = Instantiate(VFX_damage, transform.position, Quaternion.identity);
        }*/
    }

/*    public void Heal(float health)
    {
        _currentHealth += health;
        _healthBar.UpdateHealthBar(_maxHealth, _currentHealth);
        if (_currentHealth >= _maxHealth) { _currentHealth = _maxHealth; }
    }*/
}
